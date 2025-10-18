using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugAddStatusButton : MonoBehaviour
{
    public TMP_Dropdown statusSelectingDropdown;
    
    public StatusManager.StatusEffectType selectedStatus;
    
    public List<StatusManager.StatusEffectType> dropdownOptions; // FIXME this is terrible! TODO scan for all status effect datas in the proejct and then put them all here or something idk

    void OnEnable()
    {
        statusSelectingDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDisable()
    {
        statusSelectingDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    void Start()
    {
        selectedStatus = dropdownOptions[0];
    }

    void OnDropdownValueChanged(int newValue)
    {
        Debug.Log(newValue);
        selectedStatus = dropdownOptions[newValue];
    }
}
