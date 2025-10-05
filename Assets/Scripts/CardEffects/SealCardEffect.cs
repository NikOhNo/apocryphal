using System.Collections.Generic;
using UnityEngine;

public class SealCardEffect : CardEffect
{
    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> stateJobs = new();
        SealCardJob sealJob = new(PlayCard);
        stateJobs.Add(sealJob);
        return stateJobs;
    }
}
