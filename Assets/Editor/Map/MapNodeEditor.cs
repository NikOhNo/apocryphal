using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MapNode))]
public class MapNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUILayout.Space();
        MapNode mapNode = (MapNode)target;
        
        serializedObject.Update();
        DisplayListWithLogic();
        serializedObject.ApplyModifiedProperties();
    }
    
    public void DisplayListWithLogic()
    {
        SerializedProperty list = serializedObject.FindProperty("nodeConnections");
        MapNode mapNode = (MapNode) target;
        EditorGUILayout.LabelField(list.name);
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            if (GUILayout.Button("Remove Connection"))
            {
                // // Debug.Log(EditorUtility.InstanceIDToObject(list.GetArrayElementAtIndex(i).objectReferenceInstanceIDValue));
                // // Debug.Log(list.GetArrayElementAtIndex(i).objectReferenceValue);
                // mapNode.RemoveConnectionEditor((MapNode)list.GetArrayElementAtIndex(i).objectReferenceValue);
                // list.DeleteArrayElementAtIndex(i);
                // // Debug.Log((MapNode)list.GetArrayElementAtIndex(i).objectReferenceValue);
                
                
                RemoveConnection(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        if (GUILayout.Button("Add connection"))
        {
            Object obj = null;// object to be selected by default
            bool allowSceneObjects = true; // is selection of scene objects allowed? yes
            string searchFilter = null;  // default search filter to apply
            int controlID = 0; // id of the control to set
            EditorGUIUtility.ShowObjectPicker<MapNode>(obj, allowSceneObjects, searchFilter, controlID);
        }
        
        // might possibly be bad practice to put it here
        // this branch will be executed when the above created ObjectPicker is closed
        if (Event.current.commandName == "ObjectSelectorClosed")
        {
            Object obj = EditorGUIUtility.GetObjectPickerObject();
            if (obj != null)
            {
                AddConnection(mapNode, obj);
            }
        }
    }

    private void AddConnection(MapNode thisNode, Object otherNodeObject)
    {
        // todo move all of these to function params
        // ^ honestly this isn't very performance critical code so it probably doesn't matter
        SerializedProperty nodeList =  serializedObject.FindProperty("nodeConnections");
        SerializedProperty connList = serializedObject.FindProperty("connectors");
        
        
        MapNode otherNode = ((GameObject)otherNodeObject).GetComponent<MapNode>();

        if (thisNode == otherNode)
        {
            Debug.LogError("Attempted to connect a node to itself! Aborting connection creation.");
            return;
        }
        
        nodeList.InsertArrayElementAtIndex(nodeList.arraySize);
        nodeList.GetArrayElementAtIndex(nodeList.arraySize - 1).objectReferenceValue = otherNode;
        
        MapNodeConnector connector = thisNode.CreateConnectionEditor(); // creates connector and returns a ref to it
        connector.Initialize(thisNode, otherNode);
        
        connList.InsertArrayElementAtIndex(connList.arraySize);
        connList.GetArrayElementAtIndex(connList.arraySize - 1).objectReferenceValue = connector;
        
  
        SerializedObject serOtherNode = new SerializedObject(otherNode); // this is a serialized MapNode component!!
        
        serOtherNode.Update();
        SerializedProperty otherNodeList = serOtherNode.FindProperty("nodeConnections");
        SerializedProperty otherConnList = serOtherNode.FindProperty("connectors");
        
        otherNodeList.InsertArrayElementAtIndex(otherNodeList.arraySize);
        otherNodeList.GetArrayElementAtIndex(otherNodeList.arraySize - 1).objectReferenceValue = thisNode;
        otherConnList.InsertArrayElementAtIndex(otherConnList.arraySize);
        otherConnList.GetArrayElementAtIndex(otherConnList.arraySize - 1).objectReferenceValue = connector;
        serOtherNode.ApplyModifiedProperties();
    }

    private void RemoveConnection(int index)
    {
        MapNode mapNode = (MapNode) target;
        SerializedProperty nodeList = serializedObject.FindProperty("nodeConnections");
        SerializedProperty connList = serializedObject.FindProperty("connectors");
        
        // going to free this later
        MapNodeConnector connToRemove = (MapNodeConnector) connList.GetArrayElementAtIndex(index).objectReferenceValue;
        
        SerializedObject serOtherNode = new SerializedObject(nodeList.GetArrayElementAtIndex(index).objectReferenceValue);
        
        nodeList.DeleteArrayElementAtIndex(index);
        connList.DeleteArrayElementAtIndex(index);
        
        serOtherNode.Update();
        SerializedProperty otherNodeList = serOtherNode.FindProperty("nodeConnections");
        SerializedProperty otherConnList = serOtherNode.FindProperty("connectors");
        
        // are you stupid
        // find the correct index by using the connector from the first node
        int otherIndex = FindIndexOf(otherConnList, connToRemove);
        
        otherNodeList.DeleteArrayElementAtIndex(otherIndex);
        otherConnList.DeleteArrayElementAtIndex(otherIndex);
        serOtherNode.ApplyModifiedProperties();
        
        // finally just destroy the connector that we removed (not the node that would be bad)
        DestroyImmediate(connToRemove.gameObject);
    }

    // method to return the index of the mapNodeConnector conn in the list list
    // returns -1 if it does not exist
    private int FindIndexOf(SerializedProperty list, MapNodeConnector conn)
    {
        for (int i = 0; i < list.arraySize; i++)
        {
            if (list.GetArrayElementAtIndex(i).objectReferenceValue == conn)
            {
                return i;
            }
        }
        
        return -1;
    }
}
