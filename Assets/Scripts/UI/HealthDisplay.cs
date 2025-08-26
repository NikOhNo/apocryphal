using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    // * component for displaying the player's health and block in an encounter *
    
    private HealthSystem _healthSystem;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _blockText;

    public void SetHealthSystem(HealthSystem hs)
    {
        this._healthSystem = hs;
        _healthSystem.OnHealthChanged.AddListener(SetHealthText);
        _healthSystem.OnBlockChanged.AddListener(SetBlockText);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthText.text = "Health: " + _healthSystem.MaxHealth.ToString() + " / " + _healthSystem.MaxHealth.ToString();
        _blockText.text = "Block: 0";
    }
    
    void SetHealthText(int newValue)
    {
        _healthText.text = "Health: " + newValue.ToString() + " / " + _healthSystem.MaxHealth.ToString();
    }
    void SetBlockText(int newValue)
    {
        _blockText.text = "Block: " + newValue.ToString();
    }
}
