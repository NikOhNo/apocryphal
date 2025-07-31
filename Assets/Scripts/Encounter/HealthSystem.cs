using UnityEngine;
using UnityEngine.Events;

public class HealthSystem
{
    public int MaxHealth { get; private set; } 
    public int Health { get; private set; }
    public int Block { get; private set; }

    public readonly UnityEvent<int> OnHit = new();
    public readonly UnityEvent<int> OnBlockChanged = new();
    public readonly UnityEvent<int> OnHealthChanged = new();
    public readonly UnityEvent OnDeath = new();

    public void ResetHealth(int newMax)
    {
        MaxHealth = newMax;
        Health = MaxHealth;
    }

    public void GainBlock(int amount)
    {
        Block += amount;
        OnBlockChanged.Invoke(amount);
    }

    public void GainHealth(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        OnHealthChanged.Invoke(Health);
    }

    /// <summary>
    /// The health system handles the damage taken using block first, then the health.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeHit(int damage)
    {
        OnHit.Invoke(damage);

        UseBlock(damage, out int remainingDamage);

        UseHealth(remainingDamage);
        
        Debug.Log($"taking hit for {damage}");
    }

    /// <summary>
    /// Block absorbs as much damage as it can. Remaining damage is output.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="remainingDamage"></param>
    protected void UseBlock(int damage, out int remainingDamage)
    {
        remainingDamage = 0;

        Block -= damage;
        if (Block <= 0)
        {
            remainingDamage = -1 * Block;  // turns negative block into the remaining damage
            Block = 0;
        }

        OnBlockChanged.Invoke(Block);
    }

    /// <summary>
    /// Health 
    /// </summary>
    /// <param name="damage"></param>
    protected void UseHealth(int damage)
    {
        Health -= damage;

        OnHealthChanged.Invoke(Health);

        if (Health <= 0)
        {
            OnDeath.Invoke();
        }
        
        Debug.Log($"health updated to {Health}");
    }
}
