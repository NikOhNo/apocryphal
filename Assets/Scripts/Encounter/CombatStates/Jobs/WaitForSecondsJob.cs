using UnityEngine;
using UnityEngine.Events;

public class WaitForSecondsJob : IStateJob
{
    public UnityEvent OnComplete {get;} = new();
    
    private float _waitTime;

    public WaitForSecondsJob(float seconds)
    {
        this._waitTime = seconds;
    }

    public void StartJob(EncounterManager _em)
    {
        Debug.Log("Starting waiting for seconds job.");
        _em.StartWaitingForSeconds(_waitTime, OnTimeout);
    }

    public void OnTimeout()
    {
        Debug.Log("Finishing waiting for seconds job.");
        OnComplete.Invoke();
    }
}
