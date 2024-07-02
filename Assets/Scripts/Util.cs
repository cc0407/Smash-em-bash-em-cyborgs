using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static System.Random rnd = new System.Random();

    // This prints out the dictionary for debug purposes
    public static void PrintDictionary(Dictionary<string, float[]> dict)
    {
        int end;
        string line = "";
        foreach (KeyValuePair<string, float[]> entry in dict)
        {
            line += entry.Key + "\t" + "[";
            end = entry.Value.Length;
            for (int i = 0; i < end; i++)
            {
                line += entry.Value[i] + ",";
            }
            line += "]\n";
        }
        Debug.Log(line);
    }

    public static void PrintPercentages(float[] data)
    {
        string line = "[";
        foreach (float per in data)
        {
            line += per + ",";
        }
        line += "]";
        Debug.Log(line);
    }

    // Prints a generic array
    public static void PrintArray<T>(IEnumerable<T> arr)
    {
        Debug.Log(string.Join(", ", arr));
    }


    public enum states : ushort
    {
        IDLE = 0,
        BLOCK_RIGHT = 1,
        BLOCK_LEFT = 2,
        ATTACK_RIGHT = 3,
        ATTACK_LEFT = 4,
        CHARGING = 5,
        DAZED = 6,
        SUPER = 7,
        INACTIVE = 8,
    }

    // Possible counters to any player state, first param is incoming state, second param is ideal counter to it
    public static readonly Dictionary<states, states[]> counterStates = new Dictionary<states, states[]>
    {
        { states.IDLE, new states[0] },
        { states.BLOCK_RIGHT, new states[1]{ states.ATTACK_LEFT } },
        { states.BLOCK_LEFT, new states[1]{ states.ATTACK_RIGHT } },
        { states.ATTACK_RIGHT, new states[1]{ states.BLOCK_RIGHT } },
        { states.ATTACK_LEFT, new states[1]{ states.BLOCK_LEFT } },
        { states.CHARGING, new states[0] },
        { states.DAZED, new states[0] },
        { states.SUPER, new states[0] },
        { states.INACTIVE, new states[0] },
    };

    // first param is attacker state, second param is attackee state. If the attackee is in any of the states inside of the corresponding states[], then it will count as a hit
    public static readonly Dictionary<states, states[]> goodHits = new Dictionary<states, states[]>
    {
        { states.ATTACK_RIGHT, new states[5] {states.IDLE, states.BLOCK_LEFT, states.ATTACK_LEFT, states.ATTACK_RIGHT, states.DAZED} },
        { states.ATTACK_LEFT, new states[5] {states.IDLE, states.BLOCK_RIGHT, states.ATTACK_LEFT, states.ATTACK_RIGHT, states.DAZED} },
        { states.SUPER, new states[6] {states.IDLE, states.BLOCK_RIGHT, states.BLOCK_LEFT, states.ATTACK_LEFT, states.ATTACK_RIGHT, states.DAZED} },
    };

    // Maps animator tag hashes with player states enum. Used when keeping track of current player state
    public static readonly Dictionary<int, states> animStateHash = new Dictionary<int, states>
    {
        {Animator.StringToHash("Base Layer.Idle"), states.IDLE },
        {Animator.StringToHash("Base Layer.Block Right"), states.BLOCK_RIGHT },
        {Animator.StringToHash("Base Layer.Block Left"), states.BLOCK_LEFT },
        {Animator.StringToHash("Base Layer.Attack Right"), states.ATTACK_RIGHT },
        {Animator.StringToHash("Base Layer.Jab Right"), states.ATTACK_RIGHT },
        {Animator.StringToHash("Base Layer.Attack Left"), states.ATTACK_LEFT },
        {Animator.StringToHash("Base Layer.Jab Left"), states.ATTACK_LEFT },
        {Animator.StringToHash("Base Layer.Charging"), states.CHARGING },
        {Animator.StringToHash("Base Layer.Dazed"), states.DAZED },
        {Animator.StringToHash("Base Layer.Super"), states.SUPER },
        {Animator.StringToHash("Base Layer.Inactive"), states.INACTIVE },

    };


    // Maps a digit to an array of bools, representing its 7-segment display
    //   -- 0 --
    //  |       |
    //  5       1
    //  |       |
    //   -- 6 --
    //  |       |
    //  4       2
    //  |       |
    //   -- 3 --
    public static readonly bool[,] DigitDisplay = new bool[,]
    {
        {true,  true,  true,  true,  true,  true,  false}, // digit 0
        {false, true,  true,  false, false, false, false}, // digit 1
        {true,  true,  false, true,  true,  false, true }, // digit 2
        {true,  true,  true,  true,  false, false, true }, // digit 3
        {false, true,  true,  false, false, true,  true }, // digit 4
        {true,  false, true,  true,  false, true,  true }, // digit 5
        {true,  false, true,  true,  true,  true,  true }, // digit 6
        {true,  true,  true,  false, false, false, false}, // digit 7
        {true,  true,  true,  true,  true,  true,  true }, // digit 8
        {true,  true,  true,  true,  false, true,  true }  // digit 9
    };


    // Attempts to play an audioSource
    public static void AttemptPlay(AudioSource a)
    {
        if (a != null && a.clip != null)
        {
            a.Play();
        }
    }

    public static AudioClip GetRandomSound(AudioClip[] clipLibrary)
    {
        int rndIndex = Util.rnd.Next(0, clipLibrary.Length);
        if (rndIndex < clipLibrary.Length)
        {
            return (clipLibrary[rndIndex]);
        }
        else
        {
            return null;
        }
    }

    public static void PlayRandomSound(AudioClip[] lib, AudioSource a)
    {
        a.clip = GetRandomSound(lib);
        AttemptPlay(a);
    }

}
