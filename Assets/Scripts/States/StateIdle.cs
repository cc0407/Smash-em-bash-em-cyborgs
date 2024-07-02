public class StateIdle : State
{
    public StateIdle(AIController character)
    {
        name = "Idle";
        ID = Util.states.IDLE;
        transitionTable = character.GetTransitionData(name);
        transitionTableCharge = character.GetTransitionData(name + " Charge");
        GenerateAvailableNextStates();
    }
}
