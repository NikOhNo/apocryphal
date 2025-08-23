using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public HealthSystem HealthSystem { get; private set; } = new();
    
    public UnityEvent<Player> onDeath { get; private set; } = new();
    public int maxHealth;
    public HealthDisplay healthDisplay; // snet in inspector
    
    void Awake()
    {
        HealthSystem.ResetHealth(maxHealth);
        healthDisplay.SetHealthSystem(HealthSystem);
    }

    void OnEnable()
    {
        HealthSystem.OnDeath.AddListener(OnDeath);
    }

    void OnDeath()
    {
        Debug.Log("death");
        onDeath.Invoke(this);
    }
}
