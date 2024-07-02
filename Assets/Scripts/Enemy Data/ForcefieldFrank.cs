using System.Collections.Generic;

public class ForcefieldFrank : AIData
{
    public ForcefieldFrank() {
        EnemyName = "Forcefield Frank";
        FlavourText = "This bot was trained to be a back-catcher in little league baseball. Due to this, he favours defensive play, and has fast defensive reactions.";
        TransitionTable = new Dictionary<string, float[]>()
        {
            {"Idle", new float[7] {10, 30, 30, 15, 15, -1, -1 }},
            {"Idle Charge", new float[7] {10, 25, 25, 10, 10, 20, -1 }},
            {"Block Right", new float[7] {5, 25, 25, 45, -1, -1, -1 }},
            {"Block Right Charge", new float[7] {5, 20, 20, 35, -1, 20, -1 }},
            {"Block Left", new float[7] {5, 25, 25, -1, 45, -1, -1 }},
            {"Block Left Charge", new float[7] {5, 20, 20, -1, 35, 20, -1 }},
            {"Charging", new float[7] {30, -1, -1, -1, -1, 70, -1 }},
        };

        canCounterAttack = true;
        counterChance = .7f;
        counterCooldown = 0.75f; // must wait half a second before countering again
        MoveTime = 1f;
        PossibleCounterAttacks = new Util.states[2] { Util.states.BLOCK_LEFT, Util.states.BLOCK_RIGHT };

        // Changes user inputted transition table into one the computer can use
        ConvertTransitionTable();
    }
}
