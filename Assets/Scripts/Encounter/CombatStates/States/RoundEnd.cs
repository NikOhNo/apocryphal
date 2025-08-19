using UnityEngine;

public class RoundEnd : CombatState
{
    public override StateType StateType => StateType.RoundEnd;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: round end jobs

        Jobs.Enqueue(new WaitForSecondsJob(0.5f));
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
