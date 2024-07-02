using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : BotController
{
    public PlayerStatus status { get; private set; }
    private AIController aiController;
    public Util.states currentState { get; private set; }
    public Util.states prevState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        status = GetComponent<PlayerStatus>();
        anim = GetComponent<Animator>();

        startPos = new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);


        GameObject tmp = GameObject.FindWithTag("Enemy");
        if (tmp != null)
        {
            aiController = tmp.GetComponent<AIController>();
        }

        tmp = GameObject.FindWithTag("PlayerCharger");
        if (tmp != null)
        {
            ChargingBlock = tmp;
        }
    }

    /* Update is called once per frame */
    void Update()
    {
        GetStateFromAnimator();

        // Skip over any controller updates if attacking, or if dazed (IsControllable) or if match isnt started (activated)
        if (!IsControllable || !activated)
        {
            return;
        }

        if (currentState == Util.states.CHARGING)
        {
            horizontalMovement(ChargingBlock.transform.position);
        }
        else
        {
            horizontalMovement(startPos);
            // Reduce battery by 1 for every second of blocking
            if (currentState == Util.states.BLOCK_LEFT || currentState == Util.states.BLOCK_RIGHT)
            {
                status.ChangeBattery(-.5f * Time.deltaTime);
            }
        }

    }

    // Assigns new state programatically based on states enum
    // Will also run the state action once here. For continuous state actions like charging, runState() is called in the update loop
    public void ChangeState(Util.states newState)
    {
        prevState = currentState;

        currentState = newState;
    }

    public override IEnumerator StartDazed()
    {
        IsControllable = false;
        yield return new WaitForSeconds(3f);
        IsControllable = true;
    }

    public void GetStateFromAnimator()
    {
        int currentStateHash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;

        if (Util.animStateHash.ContainsKey(currentStateHash))
        {
            if (Util.animStateHash[currentStateHash] != currentState)
            {
                prevState = currentState;
                currentState = Util.animStateHash[currentStateHash];
                aiController.TryCounter(currentState);

                InitialStateStuff(currentState);
            }
        }
        else
        {
            Debug.LogError("COULDNT FIND A MATCH FROM PLAYER ANIMATION STATE. VERIFY ANIMATOR TAG NAMES ALL MATCH AN AVAILABLE STATE!");
        }
    }

    // These are things that run once, at the beginning of the state
    public void InitialStateStuff(Util.states cur)
    {
        switch (cur)
        {
            case Util.states.SUPER:
                StartCoroutine(aiController.StartSupered());
                status.EmptyBlocks();
                //Debug.Log("Attempting to super enemy");
                //Debug.Log(aiController);
                break;
            case Util.states.ATTACK_RIGHT:
            case Util.states.ATTACK_LEFT:
                status.TryDamage(aiController.currentState.ID, this.currentState);
                break;
            default:
                break;

        }
    }

}