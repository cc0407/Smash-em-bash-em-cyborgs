public class StateAttackRight : State
{
    public StateAttackRight(AIController character)
    {
        name = "Attack Right";
        ID = Util.states.ATTACK_RIGHT;
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
        else if (character.PrevState.name == "Block Right")
        {
            character.ChangeState(Util.states.BLOCK_RIGHT);
        }
        else
        {
            character.ChangeState(Util.states.IDLE);
        }
    }
}
