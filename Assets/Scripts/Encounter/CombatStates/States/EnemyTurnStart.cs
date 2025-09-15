using UnityEngine;

public class EnemyTurnStart : CombatState
{
    public override StateType StateType => StateType.EnemyTurnStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: perform enemy turn start jobs
        
        // tell enemies that their turn is starting...
        // encounter.enemyManager.OnEnemyTurnStart(); // DONT DO THIS IT'S BAD!! TERRIBLE I SAY
        
        Jobs.Enqueue(new WaitForSecondsJob(0.5f));
        Jobs.Enqueue(new CallFunctionJob(Exit));

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
