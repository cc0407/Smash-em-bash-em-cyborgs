public class StateBlockLeft : State
{
    public StateBlockLeft(AIController character)
    {
        name = "Block Left";
        ID = Util.states.BLOCK_LEFT;
        transitionTable = character.GetTransitionData(name);
        transitionTableCharge = character.GetTransitionData(name + " Charge");
        GenerateAvailableNextStates();
    }
}
