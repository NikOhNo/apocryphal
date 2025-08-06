using UnityEngine;

public class EnemyTurn : CombatState
{
    public override StateType StateType => StateType.EnemyTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: enemy turn jobs
        
        Jobs.Enqueue(new WaitForSecondsJob(_encounter, 0.5f)); // woah.
        Jobs.Enqueue(new EndStateJob(Exit)); // FIXME temporary

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
