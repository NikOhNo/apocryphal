using UnityEngine;

public class EnemyTurn : CombatState
{
    public override StateType StateType => StateType.EnemyTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: enemy turn jobs
        encounter.enemyManager.OnEnemyTurn();
        
        // Jobs.Enqueue(new WaitForSecondsJob(0.5f)); // woah.
        // Jobs.Enqueue(new CallFunctionJob(Exit)); // FIXME temporary

        // Exit();
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
