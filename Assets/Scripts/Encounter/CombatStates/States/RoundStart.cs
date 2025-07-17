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
        PerformNextJob();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override bool CanExit()
    {
        return _jobs.Count == 0;
    }

    protected virtual void QueueJobs()
    {
        // jobs.enqueue(() => _encounter.DeckSystem.DrawCards(5));...
    }

    protected virtual void PerformNextJob()
    {
        if (_jobs.Count > 0)
        {
            IStateJob nextJob = _jobs.Dequeue();
            nextJob.StartJob(PerformNextJob);
        }
        else
        {
            Exit();
        }
    }
}
