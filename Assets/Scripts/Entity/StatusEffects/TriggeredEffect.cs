using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TriggeredEffect
{
    
    public StateType activePhase;
    public StatusEffect effect; // reference to the effect in case we need to change something on it
    
    // TODO the actual effect
    // could probably do an inheritance moment here
    public abstract List<IStateJob> GetJobs(StatusEffect effect);
}
