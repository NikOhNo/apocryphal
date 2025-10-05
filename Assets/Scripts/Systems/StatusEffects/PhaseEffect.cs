using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PhaseEffect
{
    public StateType activePhase;
    // TODO the actual effect
    // could probably do an inheritance here
    public abstract List<IStateJob> GetJobs(int stacks);
}
