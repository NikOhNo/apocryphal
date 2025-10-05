using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public HealthSystem HealthSystem { get; private set; } = new();
    public StatusManager StatusManager { get; private set; } = new();
    
    public UnityEvent<Entity> OnDeath { get; private set; } = new();
    public int maxHealth;
    public HealthDisplay healthDisplay; // snet in inspector
    public StatusDisplay statusDisplay;
    
    void Awake()
    {
        HealthSystem.ResetHealth(maxHealth);
        if (healthDisplay != null)
        {
            healthDisplay.SetHealthSystem(HealthSystem);
        }
        if (statusDisplay != null)
        {
            statusDisplay.SetStatusManager(StatusManager);
        }
    }

    void OnEnable()
    {
        HealthSystem.OnDeath.AddListener(OnHealthSystemDeath);
    }

    void OnHealthSystemDeath()
    {
        Debug.Log("death");
        OnDeath.Invoke(this);
    }
}
