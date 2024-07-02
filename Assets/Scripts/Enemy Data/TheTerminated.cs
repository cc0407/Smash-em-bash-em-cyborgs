using System.Collections.Generic;

public class TheTerminated : AIData
{

    public TheTerminated()
    {
        EnemyName = "The Terminated";
        FlavourText = "This bot had its ocular circuits removed after stealing his owners motor oil. He’s been used as a training dummy for years and has suffered severe wiring damage in his memory component.";
        TransitionTable = new Dictionary<string, float[]>()
        {
            {"Idle", new float[7] {10, 25, 25, 20, 20, -1, -1 }},
            {"Idle Charge", new float[7] {5, 20, 20, 15, 15, 25, -1 }},
            {"Block Right", new float[7] {25, 10, 25, 40, -1, -1, -1 }},
            {"Block Right Charge", new float[7] {20, 10, 20, 35, -1, 15, -1 }},
            {"Block Left", new float[7] {25, 10, 25, -1, 40, -1, -1 }},
            {"Block Left Charge", new float[7] {20, 10, 20, -1, 35, 15, -1 }},
            {"Charging", new float[7] {50, -1, -1, -1, -1, 50, -1 }},
        };

        canCounterAttack = false;
        counterChance = 0;
        counterCooldown = 1f; // doenst matter because it will never counter
        MoveTime = 1.5f;
        PossibleCounterAttacks = new Util.states[0]; // This bot can't counter attack

        // Changes user inputted transition table into one the computer can use
        ConvertTransitionTable();
    }
}
