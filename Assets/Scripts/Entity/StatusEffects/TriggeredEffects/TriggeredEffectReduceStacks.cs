using System.Collections.Generic;
using UnityEngine;

public class TriggeredEffectReduceStacks: TriggeredEffect
{
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
        
        var list = new List<IStateJob>(); // lowk might not even have any jorbs

        if (amountType == AmountType.Flat)
        {
            effect.AddStacks(-amountNumber);
        }
        else if (amountType == AmountType.Percent)
        {
            var total = effect.Stacks;
            var reduction = Mathf.RoundToInt(0.01f * amountNumber * total);
            Debug.Log($"stack reduction amount: {reduction}");
            effect.AddStacks(-reduction);
        }
        return list; // looks inside no jobs
    }
}
