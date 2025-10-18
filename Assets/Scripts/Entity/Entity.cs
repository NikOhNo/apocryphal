using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public HealthSystem HealthSystem { get; private set; } = new();
    public StatusManager StatusManager { get; private set; }
    public DamageCalculator DamageCalculator { get; private set; } = new();

    public UnityEvent<Entity> OnDeath { get; private set; } = new();
    public int maxHealth;
    public HealthDisplay healthDisplay; // snet in inspector
    public StatusDisplay statusDisplay;

    [SerializeField] private EncounterManager _encounterManager; // BAD PRACTICE but set this in the inspector for fun
    
    void Awake()
    {
        StatusManager = new();
        HealthSystem.ResetHealth(maxHealth);
        if (healthDisplay != null)
        {
            healthDisplay.SetHealthSystem(HealthSystem);
        }
        if (statusDisplay != null)
        {
            statusDisplay.SetStatusManager(StatusManager);
        }
        
        DamageCalculator.SetHealthSystem(HealthSystem);
    }

    void OnEnable()
    {
        HealthSystem.OnDeath.AddListener(OnHealthSystemDeath);
        if (_encounterManager != null)
        {
            _encounterManager.OnStateChange.AddListener(OnCombatState);
        }
    }

    public void OnCombatState(ICombatState state)
    {
        StatusManager.OnEncounterState(state.StateType, _encounterManager);
    }

    void OnHealthSystemDeath()
    {
        Debug.Log("death");
        OnDeath.Invoke(this);
    }

    public void SetEncounter(EncounterManager em)
    {
        this._encounterManager = em;
        _encounterManager.OnStateChange.AddListener(OnCombatState);
    }

    public void OnDisable()
    {
        if (_encounterManager != null)
        {
            _encounterManager.OnStateChange.RemoveListener(OnCombatState);
        }
    }
}
