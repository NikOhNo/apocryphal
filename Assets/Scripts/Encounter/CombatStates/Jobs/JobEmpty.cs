using System;
using UnityEngine;
using UnityEngine.Events;

public class JobEmpty : IStateJob
{
    public UnityEvent OnComplete { get; } = new();

    public void StartJob(EncounterManager _em)
    {
        OnComplete?.Invoke();
    }
}
