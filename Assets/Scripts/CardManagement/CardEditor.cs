using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    SerializedProperty spriteProp;
    SerializedProperty hasAPCostProp;
    SerializedProperty apCostProp;
    SerializedProperty hasMPCostProp;
    SerializedProperty mpCostProp;
    SerializedProperty cardTextProp;
    
    SerializedProperty cardEffectsProp;

    void OnEnable()
    {
        spriteProp = serializedObject.FindProperty("sprite");
        hasAPCostProp = serializedObject.FindProperty("hasAPCost");
        apCostProp = serializedObject.FindProperty("APCost");
        hasMPCostProp = serializedObject.FindProperty("hasMPCost");
        mpCostProp = serializedObject.FindProperty("MPCost");
        cardTextProp = serializedObject.FindProperty("description");
        
        cardEffectsProp = serializedObject.FindProperty("effects");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        // serializedObject.Update();
        //
        // EditorGUILayout.PropertyField(cardTextProp);
        //
        // EditorGUILayout.PropertyField(spriteProp);
        //
        // EditorGUILayout.PropertyField(hasAPCostProp);
        // if (hasAPCostProp.boolValue)
        // {
        //     EditorGUILayout.PropertyField(apCostProp);
        // }
        //
        // EditorGUILayout.PropertyField(hasMPCostProp);
        // if (hasMPCostProp.boolValue)
        // {
        //     EditorGUILayout.PropertyField(mpCostProp);
        // }
        //
        // EditorGUILayout.PropertyField(cardEffectsProp);
        //
        // serializedObject.ApplyModifiedProperties();
    }
}
