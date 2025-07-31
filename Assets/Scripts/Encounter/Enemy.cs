using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    
    private HealthSystem _healthSystem = new();
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _blockText;
    
    public UnityEvent<Enemy> onDeath;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthText.text = "Health: " + maxHealth.ToString();;
        _blockText.text = "Block: 0";
        _healthSystem.ResetHealth(maxHealth);
    }

    void OnEnable()
    {
        _healthSystem.OnHealthChanged.AddListener(SetHealthText);
        _healthSystem.OnBlockChanged.AddListener(SetBlockText);
        _healthSystem.OnDeath.AddListener(OnDeathEvent);
    }

    void OnDisable()
    {
        _healthSystem.OnHealthChanged.RemoveListener(SetHealthText);
        _healthSystem.OnBlockChanged.RemoveListener(SetBlockText);
        _healthSystem.OnDeath.RemoveListener(OnDeathEvent);
    }

    void OnDeathEvent()
    {
        onDeath.Invoke(this);
    }
    void SetHealthText(int newValue)
    {
        _healthText.text = "Health: " + newValue.ToString();
    }
    void SetBlockText(int newValue)
    {
        _blockText.text = "Block: " + newValue.ToString();
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("enemy taking damage");
        _healthSystem.TakeHit(damage);
    }
    
}
