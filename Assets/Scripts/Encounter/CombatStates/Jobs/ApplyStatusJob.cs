using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ApplyStatusJob : IStateJob
{ 
    public UnityEvent OnComplete { get; } = new();
    
    private int _stacks;
    private StatusEffectData _statusEffect;
    
    public ApplyStatusJob(int stacks) { _stacks = stacks; }

    public void StartJob(EncounterManager em)
    {
        Debug.Log("Apply status job started");
        em.player.StatusManager.AddEffect(_statusEffect.effect, _stacks);
        OnComplete?.Invoke();
    }
}
