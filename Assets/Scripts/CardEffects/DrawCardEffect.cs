using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawCardEffect : CardEffect
{
    public int numCardsDrawn;

    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> jobs = new();
        DrawCardJob drawCardJob = new(numCardsDrawn);
        jobs.Add(drawCardJob);
        return jobs;
    }
}
