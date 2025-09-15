using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMoveEffectDamage : EnemyMoveEffect
{
    public int damage;
    
    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> jobs = new List<IStateJob>();
        
        jobs.Add(new DamagePlayerJob(damage));
        
        return jobs;
    }
}
