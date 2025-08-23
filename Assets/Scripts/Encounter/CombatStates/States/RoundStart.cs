using System;
using UnityEngine;
using UnityEngine.Events;

public class RoundStart : CombatState
{
    public override StateType StateType => StateType.RoundStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        QueueJobs();
    }

    protected virtual void QueueJobs()
    {
        Jobs.Enqueue(new DrawCardJob(_encounter.hand.size));
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
