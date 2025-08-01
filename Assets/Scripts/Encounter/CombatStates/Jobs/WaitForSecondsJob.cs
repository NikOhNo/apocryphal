using UnityEngine;
using UnityEngine.Events;

public class WaitForSecondsJob : IStateJob
{
    public UnityEvent OnComplete {get;} = new();
    
    private EncounterManager _encounterManager;
    private float _waitTime;

    public WaitForSecondsJob(EncounterManager encounterManager, float seconds)
    {
        this._encounterManager = encounterManager;
        this._waitTime = seconds;
    }

    public void StartJob()
    {
        Debug.Log("Starting waiting for seconds job.");
        _encounterManager.StartWaitingForSeconds(_waitTime, OnTimeout);
    }

    public void OnTimeout()
    {
        Debug.Log("Finishing waiting for seconds job.");
        OnComplete.Invoke();
    }
}
