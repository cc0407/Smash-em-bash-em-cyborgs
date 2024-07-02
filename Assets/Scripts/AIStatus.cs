using System;
using System.Linq;
using UnityEngine;

public class AIStatus : MonoBehaviour
{

    public float maxBattery { get; set; }
    public float battery { get; set; }
    public float lowBatteryThreshold { get; set; }
    private PlayerStatus ps;
    public ScoreDisplay scoreDisplay;

    public GameObject batteryIndicator;

    public AudioSource miscAudioSource;
    public AudioClip[] hitClips;
    public AudioClip[] blockClips;

    public GameObject[] audience;

    public int score { get; set; } = 0;

    void Start()
    {
        maxBattery = 10f;
        battery = maxBattery;
        lowBatteryThreshold = maxBattery * 0.4f;

        batteryIndicator = GameObject.Find("EnemyBattery");

        GameObject tmp = GameObject.FindWithTag("Player");
        if (tmp != null)
        {
            ps = tmp.GetComponent<PlayerStatus>();
        }
        tmp = GameObject.FindWithTag("EnemyScore");
        if (tmp != null)
        {
            scoreDisplay = tmp.GetComponent<ScoreDisplay>();
        }

        audience = GameObject.FindGameObjectsWithTag("Audience");
    }

    public bool IsLowBattery() { return battery < lowBatteryThreshold; }
    public bool IsLowBattery(float batteryAmt) { return batteryAmt < lowBatteryThreshold; }
    public bool IsFullBattery() { return battery >= maxBattery; }
    public bool IsDeadBattery() { return battery <= 0; }

    // Gets broadcasted to when colliding with charger
    public virtual void ChangeBattery(float amt)
    {

        // this must happen first or else it may return before updating
        // calls + amt since we havent changed battery amt yet
        if (batteryIndicator != null)
        {
            batteryIndicator.BroadcastMessage("UpdateIndicatorColour", IsLowBattery(battery + amt));
            batteryIndicator.BroadcastMessage("UpdateIndicatorDisplay", battery + amt);
        }

        if (amt < 0) // subtracting battery
        {
            if (battery <= 0) // battery already empty, dont change
                return;
            else if (battery + amt <= 0) // min clamp
            {
                BroadcastMessage("StartDazed");
                battery = 0; return;
            }
        }
        else // adding battery
        {
            if (battery >= maxBattery) // battery already full, dont change
                return;
            else if (battery + amt >= maxBattery) // max clamp
            {
                battery = maxBattery; return;
            }
        }

        battery += amt; // change battery here
    }

    public virtual void TryDamage(Util.states AIstate, Util.states playerState)
    {
        // Attempting an attack lowers your battery too
        ChangeBattery(-0.25f);

        // Check if player is in a compromising position
        if (Util.goodHits[AIstate].Contains(playerState))
        {
            //Debug.Log("thats a hit!");
            ps.ChangeBattery(-1f);
            addScore(1);
            PlayHitSound();
            AudienceReact();


        }
        // Check if player blocked this attack
        else if (Util.counterStates[AIstate].Contains(playerState))
        {
            ps.ChangeBattery(-0.25f);
            ps.addScore(1);
            ps.AddSuccessfulBlock();
            ps.PlayBlockSound();
            AudienceReact();
        }
    }

    public virtual void addScore(int amt)
    {
        score += amt;
        scoreDisplay.Display(score);
    }

    public void PlayBlockSound()
    {
        Util.PlayRandomSound(blockClips, miscAudioSource);
    }

    public void PlayHitSound()
    {
        Util.PlayRandomSound(hitClips, miscAudioSource);
    }

    public void AudienceReact()
    {
        foreach (GameObject g in audience)
        {
            g.BroadcastMessage("React");
        }
    }
}
