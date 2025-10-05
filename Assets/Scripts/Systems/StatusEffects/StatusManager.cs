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
    public void OnEncounterState(StateType stateType, EncounterManager em)
    {
        foreach (StatusEffect contained in StatusEffects)
        {
            Debug.Log(stateType);
            Debug.Log(em);
            contained.OnPhase(stateType, em);
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

    private bool HasEffect(StatusEffectType st)
    {
        foreach (StatusEffect contained in StatusEffects)
        {
            if (contained.StatusType == st)
            {
                Debug.Log("hi");
                return true;
            }
        }
        Debug.Log("bye");
        return false;
    }

    // call when you want to add an effect with x stacks (or add x stacks to an effect) to this status manager
    // will add the effect to the effect list if the affected entity doesn't have the effect yet.
    public void AddEffect(StatusEffectType statusType, int stacks)
    {
        if (!HasEffect(statusType))
        {
            // create effect of that type.
            StatusEffect createdEffect = CreateEffectOfType(statusType);
            createdEffect.StatusType = statusType;
            StatusEffects.Add(createdEffect);
            createdEffect.AddStacks(stacks - 1);
            statusAdded?.Invoke(createdEffect);
            Debug.Log($"created status effect {createdEffect}");
        }
        else
        {
            foreach (StatusEffect contained in StatusEffects)
            {
                if (contained.StatusType == statusType)
                {
                    contained.AddStacks(stacks);
                    statusStackUpdated?.Invoke(contained);
                }
            }
        }
    }

    private StatusEffect CreateEffectOfType(StatusEffectType effectType)
    {
        // follow the NAMING SCHEME or else (name your StatusEffectData scriptable object like "Status[Name]" where you replace [Name] with the name of the status
        string fileName = "Status" + effectType.ToString();
        StatusEffectData effectData = Resources.Load<StatusEffectData>("Statuses/" + fileName);
        return new StatusEffect(effectData);
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
