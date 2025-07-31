using System.Collections.Generic;
using UnityEngine;

public class JobRunner : MonoBehaviour
{
    private bool _jobRunning = false;
    
    private Queue<IStateJob> _currentJobs; // reference to the queue on the state.... bad practice? probably

    public void SwitchState(ICombatState state)
    {
        _currentJobs = state.Jobs;
        _jobRunning = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_currentJobs == null) return;
        
        if (!_jobRunning)
        {
            if (_currentJobs.Count > 0)
            {
                _jobRunning = true;
                StartNextJob();
            }
        }
    }

    private void StartNextJob()
    {
        if (_currentJobs.Count > 0)
        {
            IStateJob job = _currentJobs.Dequeue();
            job.OnComplete.AddListener( () => StartNextJob() );
            job.StartJob();
        }
        else
        {
            _jobRunning = false;
        }
    }
}
