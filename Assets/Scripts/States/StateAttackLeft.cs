public class StateAttackLeft : State
{
    public StateAttackLeft(AIController character)
    {
        name = "Attack Left";
        ID = Util.states.ATTACK_LEFT;
        transitionTable = null;
        transitionTableCharge = null;
        GenerateAvailableNextStates();
    }

    public override void Execute(AIController character)
    {
        if (character.IsDeadBattery)
        {
            character.ChangeState(Util.states.DAZED);
        }
        else if (character.PrevState.name == "Block Left")
        {
            character.ChangeState(Util.states.BLOCK_LEFT);
        }
        else
        {
            character.ChangeState(Util.states.IDLE);
        }
    }
}
