using System.Collections.Generic;

public class JumpingJack : AIData
{

    public JumpingJack()
    {
        EnemyName = "Jumping Jack";
        FlavourText = "This bot was trained by a billion kangaroos over his multi-century life. He delivers fast attacks, and cares little for his battery level.";
        TransitionTable = new Dictionary<string, float[]>()
        {
            {"Idle", new float[7] {10, 15, 15, 30, 30, -1, -1 }},
            {"Idle Charge", new float[7] {10, 15, 15, 25, 25, 10, -1 }},
            {"Block Right", new float[7] {15, 5, 20, 60, -1, -1, -1 }},
            {"Block Right Charge", new float[7] {10, 5, 15, 60, -1, 10, -1 }},
            {"Block Left", new float[7] {15, 5, 20, -1, 60, -1, -1 }},
            {"Block Left Charge", new float[7] {10, 5, 15, -1, 60, 10, -1 }},
            {"Charging", new float[7] {60, -1, -1, -1, -1, 40, -1 }},
        };

        canCounterAttack = true;
        counterChance = 0.5f;
        counterCooldown = 2f; // must wait at least 2 seconds before countering again
        MoveTime = 1f;
        PossibleCounterAttacks = new Util.states[4] { Util.states.ATTACK_LEFT, Util.states.ATTACK_RIGHT, Util.states.BLOCK_RIGHT, Util.states.BLOCK_LEFT };

        // Changes user inputted transition table into one the computer can use
        ConvertTransitionTable();
    }
}
