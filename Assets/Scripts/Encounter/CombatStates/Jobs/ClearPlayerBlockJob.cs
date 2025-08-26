using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClearPlayerBlockJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();
    
    public ClearPlayerBlockJob() { }

    public void StartJob(EncounterManager em)
    {
        Debug.Log("Clear player block job started");
        em.player.HealthSystem.ClearBlock();
        OnComplete?.Invoke();
    }
}
