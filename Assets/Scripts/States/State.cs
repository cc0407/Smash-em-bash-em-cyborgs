using System.Collections.Generic;

public abstract class State
{
    public string name;
    public Util.states ID;
    public float[] transitionTable; // This transition table is used when charging is unavailable
    public float[] transitionTableCharge; // This transition table is used when charging is available

    public List<Util.states> AvailableNextStates;

    // Generates a random number from [0,100) and checks the transition table to see which next state corresponds to this percentage
    public Util.states FindNextState(float[] chosenTransitionTable)
    {
        int randomPercent = Util.rnd.Next(0, 100);
        int end = chosenTransitionTable.Length;

        // while true in case the random number generates something that will not assign a new state. I don't think this will ever happen, but defensive programming doesn't hurt!
        while (true)
        {
            // Check each entry to see if the percentage falls within this state's weighting
            for (int i = 0; i < end; i++)
            {
                if (randomPercent < chosenTransitionTable[i])
                {
                    return (Util.states)i;
                }
            }
        }
    }
    public virtual void Execute(AIController character)
    {

        float[] chosenTransitionTable = transitionTable;
        if (character.IsDeadBattery)
        {
            character.ChangeState(Util.states.DAZED);
        }
        else if (character.IsLowBattery)
        {
            chosenTransitionTable = transitionTableCharge;
        }

        character.ChangeState(FindNextState(chosenTransitionTable));
    }

    // Creates a list of all available next moves
    // Uses List instead of array as the length varies from state to state
    public virtual void GenerateAvailableNextStates()
    {
        AvailableNextStates = new List<Util.states>();
        if (transitionTable != null)
        {
            int end = transitionTable.Length;
            for (int i = 0; i < end; i++)
            {
                if (transitionTable[i] != -1)
                {
                    AvailableNextStates.Add((Util.states)i);
                }
            }
        }
    }
}
