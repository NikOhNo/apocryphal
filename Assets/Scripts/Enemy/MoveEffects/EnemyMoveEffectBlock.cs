using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMoveEffectBlock : EnemyMoveEffect
{
    public int block;
    
    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> jobs = new List<IStateJob>();
        
        jobs.Add(new AddEnemyBlockJob(EnemyManager.EnemyTargetMode.First, block));
        
        return jobs;
    }
}
