using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class StatusEffect
{
    // current stacks of the statuseffect object initialized to 1 because usually you'd want to have 1 stack to start with
    public int Stacks { get; protected set; } = 1; 
    
    public List<TriggeredEffect> triggeredEffects = new List<TriggeredEffect>();
    public List<PersistentEffect> persistentEffects;
    
    public StatusManager.StatusEffectType StatusType { get; set; }
    
    public UnityEvent expire = new UnityEvent();
    public UnityEvent<int> stacksUpdated = new UnityEvent<int>();
    
    public bool stackable;
    public int maxStacks;

    public StatusEffect(StatusEffectData data)
    {
        triggeredEffects = data.phaseEffects;
        stackable = data.stackable;
        maxStacks = data.maxStacks;
    }
    
    // call when entering a state.
    // inheritors should define this method with the effects they want to perform for each state e.g. doing damage at the end of the player's turn
    // this allows effects to do different things at different parts of the combat
    public void OnPhase(StateType stateType, EncounterManager em)
    {
        foreach (var phaseEffect in triggeredEffects)
        {
            if (stateType == phaseEffect.activePhase)
            {
                // TODO run the effect, whatever it is
                var jobs = phaseEffect.GetJobs(this);
                foreach (IStateJob j in jobs)
                {
                    em.jobRunner.QueueJob(j);
                }
            }
        }
    } 


    public void OnTrigger()
    {
        
    }
    
    public void AddStacks(int stacks)
    {
        if (stackable)
        {
            this.Stacks = Mathf.Clamp(this.Stacks + stacks, 0, this.maxStacks);
            stacksUpdated.Invoke(this.Stacks);
        }
    }
}
