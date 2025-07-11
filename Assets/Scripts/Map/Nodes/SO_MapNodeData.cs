using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapNodeData")]
public class SO_MapNodeData : ScriptableObject
{
    [Tooltip("Scene to load when traveling to this node")] public SceneAsset onTravelScene;
    
    // TODO: add a List of scenes to load, and randomly select from that list (with perhaps preset weights?) when traveling there.
}
