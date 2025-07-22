using UnityEngine;
using UnityEditor;

public class MapTools
{
    [MenuItem("Tools/Map/Clear All Connections")]
    private static void ClearAllConnections()
    {
        int removedConnections = 0;
        MapNode[] allNodesInScene = Object.FindObjectsByType<MapNode>(FindObjectsSortMode.None);
        foreach (MapNode n in allNodesInScene)
        {
            // remove all elements in their nodeConnections and connectors lists
            SerializedObject so = new SerializedObject(n);
            
            so.Update();
            
            SerializedProperty nodes = so.FindProperty("nodeConnections");
            SerializedProperty conns = so.FindProperty("connectors");
            
            removedConnections += nodes.arraySize;
            
            nodes.ClearArray();
            conns.ClearArray();
            
            so.ApplyModifiedProperties();
        }
        
        
        MapNodeConnector[] allConnectorsInScene = Object.FindObjectsByType<MapNodeConnector>(FindObjectsSortMode.None);
        foreach (MapNodeConnector c in allConnectorsInScene)
        {
            GameObject.DestroyImmediate(c.gameObject);
        }
        
        Debug.Log($"Removed {removedConnections / 2} connections across {allNodesInScene.Length} nodes");
    }
}
