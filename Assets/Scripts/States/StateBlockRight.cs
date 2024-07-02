public class StateBlockRight : State
{
    public StateBlockRight(AIController character)
    {
        name = "Block Right";
        ID = Util.states.BLOCK_RIGHT;
        transitionTable = character.GetTransitionData(name);
        transitionTableCharge = character.GetTransitionData(name + " Charge");
        GenerateAvailableNextStates();
    }
}
