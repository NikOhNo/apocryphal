using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager
{
    // enum of all the status effects in the game :D
    // todo might want to automate this with a tool script or something
    public enum StatusEffectType
    {
        Vulnerable,
        Weakened,
        Burning
    }
    
    public List<StatusEffect> StatusEffects { get; private set; } = new List<StatusEffect>();
    
    public UnityEvent<StatusEffect> statusStackUpdated = new UnityEvent<StatusEffect>();
    public UnityEvent<StatusEffect> statusAdded = new UnityEvent<StatusEffect>();
    public UnityEvent<StatusEffect> statusRemoved = new UnityEvent<StatusEffect>();

    // call from the entity that this statusmanager is attached to every time there's a state change
    public void OnEncounterState(StateType stateType)
    {
        foreach (StatusEffect contained in StatusEffects)
        {
            contained.OnPhase(stateType);
        }
    }

    public bool HasEffectOfType(StatusEffectType type)
    {
        foreach (StatusEffect contained in StatusEffects)
        {
            if (contained.StatusType == type)
            {
                return true;
            }
        }
        return false;
    }

    // call when you want to add an effect with x stacks (or add x stacks to an effect) to this status manager
    // will add the effect to the effect list if the affected entity doesn't have the effect yet.
    public void AddEffect(StatusEffect effect, int stacks)
    {
        foreach (StatusEffect contained in StatusEffects)
        {
            if (contained.StatusType == effect.StatusType)
            {
                contained.AddStacks(stacks);
                statusStackUpdated?.Invoke(contained);
            }
            else
            {
                StatusEffects.Add(effect);
                effect.AddStacks(stacks - 1);  
                statusAdded?.Invoke(effect);
            }
        }
    }

    public void RemoveEffect(StatusEffectType effectType)
    {
        foreach (StatusEffect contained in StatusEffects)
        {
            if (contained.StatusType == effectType)
            {
                StatusEffects.Remove(contained);
                // hope and pray there's no more effects of the same type in the list
                statusRemoved?.Invoke(contained);
                return;
            }
        }
    }
}
