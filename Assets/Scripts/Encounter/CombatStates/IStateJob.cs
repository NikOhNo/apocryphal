using System;
using UnityEngine;
using UnityEngine.Events;

public interface IStateJob
{
    public UnityEvent OnComplete { get; }
    public void StartJob(EncounterManager encounterManager);
}
