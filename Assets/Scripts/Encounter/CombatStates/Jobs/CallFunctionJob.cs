using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CallFunctionJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();
    
    public Action Callback;

    public CallFunctionJob(Action callback)
    {
        Callback = callback;
    }

    public void StartJob(EncounterManager _em)
    {
        Debug.Log($"Call function job has started with function {Callback.Method.Name}.");
        Callback?.Invoke();
        OnComplete?.Invoke();
    }
}
