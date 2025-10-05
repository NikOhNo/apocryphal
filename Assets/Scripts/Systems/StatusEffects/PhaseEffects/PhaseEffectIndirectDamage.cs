using System.Collections.Generic;
using UnityEngine;

public class PhaseEffectIndirectDamage : PhaseEffect
{
    // (based on stacks)

    public enum AmountType
    {
        Flat,
        Percent
    }

    public AmountType amountType;
    public int amountNumber;

    public override List<IStateJob> GetJobs(int stacks)
    {
        var list = new List<IStateJob>();

        int damageAmount = 0;
        if (amountType == AmountType.Flat)
        {
            damageAmount = amountNumber;
        }
        if (amountType == AmountType.Percent)
        {
            damageAmount = Mathf.RoundToInt(amountNumber * 0.01f * stacks);
        }

        list.Add(new DamagePlayerJob(damageAmount)); // FIXME CURRENTLY ONLY DAMAGES THE PLAYER
        return list;
    }
}
