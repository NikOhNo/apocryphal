using UnityEngine;

public class PlayerTurnStart : CombatState
{
    public override StateType StateType => StateType.PlayerTurnStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        _encounter.transitionHandler.HandleTransition(StateType);
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
