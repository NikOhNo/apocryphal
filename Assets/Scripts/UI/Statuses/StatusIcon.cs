using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public StatusManager.StatusEffectType statusEffectType;
    
    private TextMeshProUGUI stackCount;
    private Image statusIcon;
    
    public void Awake()
    {
        stackCount = GetComponentInChildren<TextMeshProUGUI>();
        statusIcon = GetComponentInChildren<Image>();
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

    public void SetIcon(Sprite img)
    {
        statusIcon.sprite = img;
    }
}
