using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    private HealthSystem attachedHealthSystem;
    
    // should probably have a list of outgoingmodifyingeffects or something
    // and the same for all incomingmodifyingeffects
    // when a damage is dealt just iterate through all the effects, doing all the arithmetic to arrive at the final val
    
    public List<DamageModifier> outgoingDamageModifiers;
    public List<DamageModifier> incomingDamageModifiers;

    public void SetHealthSystem(HealthSystem hs)
    {
        this.attachedHealthSystem = hs;
    }
    
    // theoretically called by any attacks/effects/cards/whatever that deals damage FROM this entity
    public void DealOutgoingDamage(Entity target, float unmodifiedDamage)
    {
        float totalPercentModifier = 0.0f;
        float totalFlatModifier = 0.0f;
        foreach (DamageModifier modifier in outgoingDamageModifiers)
        {
            if (modifier.amountType == DamageModifier.AmountType.Percent)
            {
                totalPercentModifier += modifier.amount;
            }

            if (modifier.amountType == DamageModifier.AmountType.Flat)
            {
                totalFlatModifier += modifier.amount;
            }
        }
        
        float addedPercentDamage = unmodifiedDamage * totalPercentModifier * 0.01f;
        int finalDamage = Mathf.FloorToInt(addedPercentDamage + totalFlatModifier);
        target.DamageCalculator.ReceiveIncomingDamage(finalDamage);
    }

    public void ReceiveIncomingDamage(float unmodifiedDamage)
    {
        float totalPercentModifier = 0.0f;
        float totalFlatModifier = 0.0f;
        
        foreach (DamageModifier modifier in incomingDamageModifiers)
        {
            if (modifier.amountType == DamageModifier.AmountType.Percent)
            {
                totalPercentModifier += modifier.amount;
            }

            if (modifier.amountType == DamageModifier.AmountType.Flat)
            {
                totalFlatModifier += modifier.amount;
            }
        }
        
        float addedPercentDamage = unmodifiedDamage * totalPercentModifier * 0.01f;
        int finalDamage = Mathf.FloorToInt(addedPercentDamage + totalFlatModifier); // rounding it down for fun and profit
        attachedHealthSystem.TakeHit(finalDamage);
    }
}
