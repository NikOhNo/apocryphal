using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageRandomEnemyEffect : CardEffect
{
    public int damage;
    
    public override List<IStateJob> GetJobs()
    {
        List<IStateJob> jobs = new();
        DamageRandomEnemyJob jorb = new(damage);
        jobs.Add(jorb);
        return jobs;
    }
}
