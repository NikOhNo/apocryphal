using System;
using UnityEngine;
using UnityEngine.Events;

public class EndStateJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();
    
    public Action EndTurnCallback;

    public EndStateJob(Action endTurnCallback)
    {
        EndTurnCallback = endTurnCallback;
    }

    public void StartJob(EncounterManager _em)
    {
        Debug.Log("End turn job has started! i am now ending turn.");
        EndTurnCallback?.Invoke();
        OnComplete?.Invoke();
    }
}
