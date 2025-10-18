using UnityEngine;

public class DamageModifier
{
    public float amount; // that it modifies by

    public enum AmountType
    {
        Percent,
        Flat
    }

    public AmountType amountType { get; set; }

    public enum DamageType
    {
        Direct,
        Indirect,
        Both
    }
    
    public DamageType damageType { get; set; }
    
    public StatusEffect sourceEffect { get; private set; }
}
