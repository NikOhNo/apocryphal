using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MapNode))]
public class MapNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapNode mapNode = (MapNode)target;
        if (GUILayout.Button("Add connection"))
        {
            // Debug.Log("what");
            
            Object obj = null;// object to be selected by default
            bool allowSceneObjects = true; // is selection of scene objects allowed? yes
            string searchFilter = null;  // default search filter to apply
            int controlID = 0; // id of the control to set
            EditorGUIUtility.ShowObjectPicker<MapNode>(obj, allowSceneObjects, searchFilter, controlID);
        }
        
        // might possibly be bad practice to put it here
        if (Event.current.commandName == "ObjectSelectorClosed")
        {
            var obj = EditorGUIUtility.GetObjectPickerObject();
            mapNode.AddConnection((GameObject)obj);
        }
    }
}
