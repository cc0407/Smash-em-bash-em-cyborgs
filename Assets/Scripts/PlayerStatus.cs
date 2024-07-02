using System;
using System.Linq;
using UnityEngine;

public class PlayerStatus : AIStatus
{
    public GameObject EnemyWrapper;
    private AIStatus aiStatus;
    public BlockIndicator bi;

    public AudioClip[] superClips;

    private int BlockCount = 0;

    void Start()
    {
        maxBattery = 10f;
        battery = maxBattery;
        lowBatteryThreshold = maxBattery * 0.4f;

        aiStatus = EnemyWrapper.transform.GetChild(0).transform.GetChild(0).GetComponent<AIStatus>();
    }

    public override void TryDamage(Util.states AIstate, Util.states playerState)
    {
        // Attempting an attack lowers your battery too
        ChangeBattery(-1f);

        // Check if AI is in a compromising position
        if (Util.goodHits[playerState].Contains(AIstate))
        {
            //Debug.Log("thats a hit for the player!");
            aiStatus.ChangeBattery(-1.5f);
            addScore(1);
            PlayHitSound();
            AudienceReact();
        }
        // Check if AI blocked this attack
        else if (Util.counterStates[playerState].Contains(AIstate))
        {
            aiStatus.ChangeBattery(-0.25f);
            aiStatus.addScore(1);
            aiStatus.PlayBlockSound();
            AudienceReact();
        }
    }

    // Increases the counter, need 5 for a super attack
    public void AddSuccessfulBlock()
    {
        if (BlockCount < 5)
            BlockCount++;
        else BlockCount = 5;

        bi.TurnOn(BlockCount);
    }

    // Removes all successful blocks. Used when super attacking.
    public void EmptyBlocks()
    {
        BlockCount = 0;
        bi.TurnOn(BlockCount);
    }

    public bool CanSuper()
    {
        return BlockCount >= 5;
    }

    public void PlaySuperSound()
    {
        Util.PlayRandomSound(superClips, miscAudioSource);
    }
}