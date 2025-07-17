using UnityEngine;

public class EnemyTurnStart : CombatState
{
    public override StateType StateType => StateType.EnemyTurnStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: perform enemy turn start jobs

        Exit();
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
