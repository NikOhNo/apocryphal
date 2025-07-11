using UnityEngine;

public class MapNodeEvent : MapNode
{
    protected override void OnTravelTo()
    {
        GetComponent<Animator>().SetTrigger("Explode");
    }
}
