using UnityEngine;

public class EnemyTurnEnd : CombatState
{
    public override StateType StateType => StateType.EnemyTurnEnd;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: Enemy Turn End jobs
        
        Jobs.Enqueue(new WaitForSecondsJob(encounter, 0.5f));
        Jobs.Enqueue(new EndStateJob(Exit));

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
