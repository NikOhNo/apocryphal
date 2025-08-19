using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private HealthSystem _healthSystem = new();
    
    
    public UnityEvent<Player> onDeath { get; private set; } = new();
    public int maxHealth;
    public HealthDisplay healthDisplay; // snet in inspector

    void Awake()
    {
        _healthSystem.ResetHealth(maxHealth);
        healthDisplay.SetHealthSystem(_healthSystem);
    }

    void OnEnable()
    {
        _healthSystem.OnDeath.AddListener(OnDeath);
    }

    void OnDeath()
    {
        Debug.Log("death");
        onDeath.Invoke(this);
    }

    public void TakeDamage(int damage)
    {
        this._healthSystem.TakeHit(damage);
    }

    public void AddBlock(int amount)
    {
        this._healthSystem.GainBlock(amount);
    }
}
