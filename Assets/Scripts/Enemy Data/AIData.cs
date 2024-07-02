using System.Collections.Generic;
using UnityEngine;

public abstract class AIData : MonoBehaviour
{
    public Dictionary<string, float[]> TransitionTable { get; protected set; }
    public bool canCounterAttack { get; protected set; }
    public float counterChance { get; protected set; }
    public Util.states[] PossibleCounterAttacks { get; protected set; } // Contains a list of counter attacks that this robot is capable of doing
    public string EnemyName { get; protected set; }
    public float MoveTime { get; protected set; }
    public float counterCooldown { get; protected set; }
    public string FlavourText { get; protected set; }


    // Converts the transition table into one that adds percents sequentially. Good for calculating the random value
    public void ConvertTransitionTable()
    {
        Dictionary<string, float[]> newTransitionTable = this.TransitionTable;
        float percent;
        int end;
        foreach (KeyValuePair<string, float[]> entry in newTransitionTable)
        {
            end = entry.Value.Length;
            percent = 0;
            for (int i = 0; i < end; i++)
            {
                // Skip over this entry if it does not have a weight
                if (entry.Value[i] == -1)
                    continue;

                percent += entry.Value[i];
                entry.Value[i] = percent;
            }
        }
        TransitionTable = newTransitionTable;
    }
}
