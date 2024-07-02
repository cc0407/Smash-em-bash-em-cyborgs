public class StateDazed : State
{
    public StateDazed(AIController character)
    {
        name = "Dazed";
        ID = Util.states.DAZED;
        transitionTable = null;
        transitionTableCharge = null;
        GenerateAvailableNextStates();
    }

    public override void Execute(AIController character)
    {
        if (!character.IsDazed)
            character.ChangeState(Util.states.IDLE);
    }
}
