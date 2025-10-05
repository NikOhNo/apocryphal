using TMPro;
using UnityEngine;

public class StatusIcon : MonoBehaviour
{
    public StatusManager.StatusEffectType statusEffectType;
    
    private TextMeshProUGUI stackCount;
    
    public void Awake()
    {
        stackCount = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SetStacks(int value)
    {
        if (value == 1)
        {
            stackCount.enabled = false;
        }
        else
        {
            stackCount.enabled = true;
            stackCount.text = value.ToString();
        }
    }
}
