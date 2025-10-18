using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Scriptable Objects/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    // could probably create a statuseffect runtime object with like...
    // the enums that are set on this 
    
    public string effectName;
    public string flavorText;
    
    public Sprite icon;
    
    //[SR, SerializeReference] public StatusEffect effect; 
    
    [SR, SerializeReference] public List<TriggeredEffect> phaseEffects; // effects that are run on a certain phase
    
    // TODO add triggerEffects which are run on certain conditions
    
    // TODO add persistent effects which are active as long as the status is applied
    
    public bool stackable;
    public int maxStacks;
}
