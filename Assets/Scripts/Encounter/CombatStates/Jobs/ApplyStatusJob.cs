using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ApplyStatusJob : IStateJob
{ 
    public UnityEvent OnComplete { get; } = new();
    
    private int _stacks;
    
    private StatusManager.StatusEffectType _selectedStatus;
    
    public ApplyStatusJob(int stacks, StatusManager.StatusEffectType selectedStatus) 
    { 
        _stacks = stacks;
        _selectedStatus = selectedStatus;
    }

    public void StartJob(EncounterManager em)
    {
        Debug.Log("Apply status job started");
        em.player.StatusManager.AddEffect(_selectedStatus, _stacks);
        OnComplete?.Invoke();
    }
}
