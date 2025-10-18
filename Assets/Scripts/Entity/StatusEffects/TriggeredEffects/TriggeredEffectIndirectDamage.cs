using System.Collections.Generic;
using UnityEngine;

public class TriggeredEffectIndirectDamage : TriggeredEffect
{
    // (based on stacks)

    public enum AmountType
    {
        Flat,
        Percent
    }

    public AmountType amountType;
    public int amountNumber;

    public override List<IStateJob> GetJobs(StatusEffect eff)
    {
        this.effect = eff;
        var list = new List<IStateJob>();
        int stacks = effect.Stacks; // effect is on the superclass

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
