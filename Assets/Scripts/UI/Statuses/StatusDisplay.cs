using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{ 
    private StatusManager _statusManager; // sm that we're gonna look at to update this display
    
    public List<StatusIcon> iconList = new List<StatusIcon>();
    
    public StatusIcon PF_StatusIcon;
    
    public GridLayoutGroup gridLayoutGroup; // set in inspector! should be a child of the object this is attached to but doesn't have to be

    public void SetStatusManager(StatusManager sm)
    {
        _statusManager = sm;
    }

    public void Awake()
    {
        if (gridLayoutGroup == null)
        {
            // fine just create the object myself
            // var go = new GameObject("StatusIconGrid");
            // go.transform.SetParent(transform);
            // gridLayoutGroup = go.AddComponent<GridLayoutGroup>();
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _statusManager.statusAdded.AddListener(OnStatusAdded);
        _statusManager.statusRemoved.AddListener(OnStatusRemoved);
        _statusManager.statusStackUpdated.AddListener(OnStatusStackUpdated);
    }

    private void OnStatusStackUpdated(StatusEffect effect, int stacks)
    {
        foreach (var icon in iconList)
        {
            if (icon.statusEffectType == effect.StatusType)
            {
                icon.SetStacks(effect.Stacks);
                return; // hope and pray that there's no duplicate effects
            }
        }
    }

    public void OnStatusAdded(StatusEffect effect)
    {
        var newIcon = Instantiate(PF_StatusIcon, gridLayoutGroup.transform);
        newIcon.statusEffectType = effect.StatusType;
        newIcon.SetIcon(_statusManager.statusDictionary[effect.StatusType].icon); // bleh
        iconList.Add(newIcon);
        newIcon.SetStacks(effect.Stacks);
    }

    public void OnStatusRemoved(StatusEffect effect)
    {
        foreach (var icon in iconList)
        {
            if (icon.statusEffectType == effect.StatusType)
            {
                iconList.Remove(icon);
                Destroy(icon); // die!
            }
        }
    }
}
