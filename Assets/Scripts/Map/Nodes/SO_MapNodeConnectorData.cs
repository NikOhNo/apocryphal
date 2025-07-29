using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapNodeConnectorData")]
public class SO_MapNodeConnectorData : ScriptableObject
{
    public SceneAsset encounterScene; // for now, this scene has a 100 percent chance to spawn when you travel over the connector.
    public float encounterChance; // proportional chance for this encounter to spawn // TODO: probably maybe don't want to add this kind of randomness... tbd
    
    // TODO: add a List of scenes to load, and randomly select from that list (with perhaps preset weights?) when traveling there.
}
