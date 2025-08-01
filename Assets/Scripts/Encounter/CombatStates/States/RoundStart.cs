using System;
using UnityEngine;
using UnityEngine.Events;

public class RoundStart : CombatState
{
    public override StateType StateType => StateType.RoundStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // QueueJobs();
        // PerformNextJob();
        
        Jobs.Enqueue(new WaitForSecondsJob(encounter, 0.5f));
        Jobs.Enqueue(new EndStateJob(Exit));
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override bool CanExit()
    {
        return Jobs.Count == 0;
    }

    protected virtual void QueueJobs()
    {
        // jobs.enqueue(() => _encounter.DeckSystem.DrawCards(5));...
    }

    protected virtual void PerformNextJob()
    {
        if (Jobs.Count > 0)
        {
            IStateJob nextJob = Jobs.Dequeue();
            nextJob.OnComplete.AddListener( () => PerformNextJob() );
            nextJob.StartJob();
        }
        else
        {
            Exit();
        }
    }
}
