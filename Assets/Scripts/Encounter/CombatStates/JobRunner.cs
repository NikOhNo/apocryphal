using System.Collections.Generic;
using UnityEngine;

public class JobRunner : MonoBehaviour
{
    private bool _jobRunning = false;
    
    private Queue<IStateJob> _currentJobs; // reference to the queue on the state.... bad practice? probably
    
    public EncounterManager encounterManager;

    public void SwitchState(ICombatState state)
    {
        _currentJobs = state.Jobs;
        _jobRunning = false;
    }

    // method to just arbitrarily queue a job
    // since the jobrunner is accessible from the encountermanager this means anything w/ a ref to the encountermanager
    // can just queue any job. useful for and executing card effects and status effects
    public void QueueJob(IStateJob job)
    {
        _currentJobs.Enqueue(job);
    }
    
    void Update()
    {
        if (_currentJobs == null) return;
        
        if (!_jobRunning)
        {
            StartNextJob();
        }
    }

    private void StartNextJob()
    {
        if (_currentJobs.Count > 0)
        {
            _jobRunning = true;
            IStateJob job = _currentJobs.Dequeue();
            job.OnComplete.AddListener(StartNextJob);
            Debug.Log("Added listener for job " + job);
            job.StartJob(encounterManager);
        }
        else
        {
            _jobRunning = false;
        }
    }
}
