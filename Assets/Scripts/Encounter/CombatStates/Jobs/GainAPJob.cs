using UnityEngine;
using UnityEngine.Events;

public class GainAPJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();

    public void StartJob(EncounterManager encounterManager)
    {
        encounterManager.playerMPAP.GainRoundAP();

        OnComplete.Invoke();
    }
}
