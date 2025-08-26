using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiscardCardsEffect : CardEffect
{
    public int amount;
    
    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> jobs = new();
        OpenCardSelectorJob jorb = new();
        jobs.Add(jorb);
        return jobs;
    }
}
