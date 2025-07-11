using UnityEngine;

public class MapNodeCombat : MapNode
{
    public string onTravelToMessage;
    
    protected override void OnTravelTo()
    {
        Debug.Log(onTravelToMessage);
    }
}
