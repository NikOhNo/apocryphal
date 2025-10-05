using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ApplyStatusJob : IStateJob
{ 
    public UnityEvent OnComplete { get; } = new();
    
    private int _stacks;
    private StatusManager.StatusEffectType statusType;
    //private StatusEffectData _statusEffect;
    
    public ApplyStatusJob(int stacks) 
    { 
        _stacks = stacks;
        statusType = StatusManager.StatusEffectType.Burning;
    }

    public void StartJob(EncounterManager em)
    {
        Debug.Log("Apply status job started");
        // FIXME just adding the status to the player AND the effect is hardcoded
        em.player.StatusManager.AddEffect(statusType, _stacks);
        OnComplete?.Invoke();
    }
}
