public class StateCharging : State
{
    public StateCharging(AIController character)
    {
        name = "Charging";
        ID = Util.states.CHARGING;
        transitionTable = character.GetTransitionData(name);
        transitionTableCharge = null;
        GenerateAvailableNextStates();
    }

    public override void Execute(AIController character)
    {

        // Get out of charging if battery is full
        if (character.IsFullBattery)
        {
            character.ChangeState(Util.states.IDLE);
        }

        // Choose to get out of charging randomly
        character.ChangeState(FindNextState(transitionTable));
    }
}
