using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AIController : BotController
{

    private float moveCooldownTimer = 0f;
    private float counterCooldownTimer = 0f;

    private PlayerController pController;
    public AIStatus aiStatus { get; private set; }
    public State currentState { get; private set; }
    public State PrevState { get; private set; }
    public AIData EnemyData { get; private set; }
    public bool CanCounter { get; set; } // If this bot is capable of counter attacking
    public bool IsLowBattery { get; set; } // Health is below 40%
    public bool IsDeadBattery { get; set; }
    public bool IsFullBattery { get; set; }
    public bool IsDazed { get; set; }

    public bool IsSupered { get; private set; }

    public GameObject retaliationReaction;

    //This is a hack for legacy animation - we will do this properly later

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        EnemyData = GetComponent<AIData>();
        aiStatus = GetComponent<AIStatus>();

        GameObject tmp = GameObject.FindWithTag("Player");
        if (tmp != null)
        {
            pController = tmp.GetComponent<PlayerController>();
        }

        tmp = GameObject.FindWithTag("EnemyCharger");
        if (tmp != null)
        {
            ChargingBlock = tmp;
        }

        startPos = new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);

        CanCounter = EnemyData.canCounterAttack;

        // Set starting state
        ChangeState(Util.states.IDLE);
    }

    void Update()
    {
        moveCooldownTimer += Time.deltaTime;
        counterCooldownTimer += Time.deltaTime;

        // if not controllable, or a blocking animation is currently being played

        CheckStateInputs();
        // if match has started
        if (activated)
        {
            // Change states based on how often enemy should be attacking
            // Check if dazed using IsControllable, and that it isnt currently getting supered
            if (moveCooldownTimer >= EnemyData.MoveTime && IsControllable && !IsSupered)
            {
                moveCooldownTimer -= EnemyData.MoveTime;
                currentState.Execute(this);
            }

            switch (currentState.ID)
            {
                case Util.states.CHARGING:
                    AIMoveToCharger();
                    break;
                case Util.states.BLOCK_LEFT:
                case Util.states.BLOCK_RIGHT:
                    // Reduce battery by 1 for every second of blocking
                    aiStatus.ChangeBattery(-.5f * Time.deltaTime);
                    AIMoveFromCharger();
                    break;
                default:
                    AIMoveFromCharger();
                    break;
            }
        }
    }

    // Assigns new state programatically based on states enum
    // Will also run the state action once here. For continuous state actions like charging, runState() is called in the update loop
    public void ChangeState(Util.states newState)
    {
        //Debug.Log(newState.ToString());
        if (currentState != null)
            PrevState = currentState;

        switch (newState)
        {
            case Util.states.BLOCK_RIGHT:
                currentState = new StateBlockRight(this);
                break;
            case Util.states.BLOCK_LEFT:
                currentState = new StateBlockLeft(this);
                break;
            case Util.states.ATTACK_RIGHT:
                currentState = new StateAttackRight(this);
                aiStatus.TryDamage(newState, pController.currentState);
                break;
            case Util.states.ATTACK_LEFT:
                currentState = new StateAttackLeft(this);
                aiStatus.TryDamage(newState, pController.currentState);
                break;
            case Util.states.CHARGING:
                currentState = new StateCharging(this);
                break;
            case Util.states.DAZED:
                currentState = new StateDazed(this);
                break;
            default:
            case Util.states.IDLE:
                currentState = new StateIdle(this);
                break;
        }
    }

    void CheckStateInputs()
    {
        IsLowBattery = aiStatus.IsLowBattery();
        IsDeadBattery = aiStatus.IsDeadBattery();
        IsFullBattery = aiStatus.IsFullBattery();
        IsDazed = false;
    }

    public float[] GetTransitionData(string s)
    {
        if (EnemyData.TransitionTable.ContainsKey(s))
            return EnemyData.TransitionTable[s];
        else
        {
            Debug.LogError(s + " does not exist in Transition Table for: " + EnemyData.EnemyName);
            return null;
        }
    }

    // Counter Attack Functionality
    public void TryCounter(Util.states s)
    {
        Util.states[] availableCounters = null;
        Util.states temp = currentState.ID;
        // If bot is able to counter
        if (CanCounter && Util.rnd.NextDouble() <= EnemyData.counterChance && counterCooldownTimer >= EnemyData.counterCooldown)
        {
            // If the incoming move is counterable, and potential counters are available next states
            if (EnemyData.PossibleCounterAttacks != null && Util.counterStates[s] != null)
            {
                // Inner join counterattacks that bot is capable of, with optimal counter moves for current player state
                availableCounters = EnemyData.PossibleCounterAttacks.Intersect(Util.counterStates[s]).ToArray();
                if (availableCounters != null && currentState.AvailableNextStates != null)
                {
                    // if there are available counter moves, check to see if we can transition into them
                    availableCounters = availableCounters.Intersect(currentState.AvailableNextStates).ToArray();
                    if (availableCounters.Length > 0)
                    {

                        ChangeState(availableCounters[0]);
                        //Debug.Log("Countering from " + temp + " to " + currentState.ID + " !");
                        counterCooldownTimer = 0f;
                        moveCooldownTimer = 0f;
                        StartCoroutine(ShowRelatiationReaction());
                    }
                }
            }
        }

    }

    public void AIMoveToCharger()
    {
        if (ChargingBlock != null && currentState.ID == Util.states.CHARGING)
        {
            horizontalMovement(ChargingBlock.transform.position);
        }
    }

    public void AIMoveFromCharger()
    {
        if (ChargingBlock != null)
        {
            horizontalMovement(startPos);
        }
    }


    public override IEnumerator StartDazed()
    {
        IsControllable = false;
        ChangeState(Util.states.DAZED);
        yield return new WaitForSeconds(3f);
        ChangeState(Util.states.CHARGING);
        moveCooldownTimer = 0;
        IsControllable = true;
    }

    public IEnumerator StartSupered()
    {
        // Current AI state will get hit by super attack
        if (Util.goodHits[Util.states.SUPER].Contains(currentState.ID))
        {
            pController.status.PlaySuperSound();
            pController.status.addScore(5);
            IsSupered = true;
            ChangeState(Util.states.DAZED);
            yield return new WaitForSeconds(.5f);
            ChangeState(Util.states.CHARGING);
            moveCooldownTimer = 0;
            IsSupered = false;
        }
    }

    // Gets broadcasted to once battery is full. Boots AI off of charger
    public virtual void AttemptBootOut()
    {
        if (aiStatus.battery >= aiStatus.maxBattery)
        {
            moveCooldownTimer = 0;
            ChangeState(Util.states.IDLE);
        }
    }

    public IEnumerator ShowRelatiationReaction()
    {
        if (retaliationReaction != null)
        {
            retaliationReaction.SetActive(true);
            yield return new WaitForSeconds(.3f);
            retaliationReaction.SetActive(false);
        }
    }
}