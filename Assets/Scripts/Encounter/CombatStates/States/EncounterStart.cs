using System;
using UnityEngine;
using UnityEngine.Events;

public class EncounterStart : CombatState
{
    public override StateType StateType => StateType.EncounterStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        QueueJobs();
    }

    protected virtual void QueueJobs()
    {
        // QueueJobs();
        // PerformNextJob();
        
        Jobs.Enqueue(new WaitForSecondsJob(0.5f));
        Jobs.Enqueue(new CallFunctionJob(Exit));
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override bool CanExit()
    {
        return Jobs.Count == 0;
    }
}
