using System.Collections.Generic;
using UnityEngine;

public class PlayerGainBlockEffect : CardEffect
{
    public int block;
    
    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> jobs = new();
        AddPlayerBlockJob jorb = new(block);
        jobs.Add(jorb);
        return jobs;
    }
}
