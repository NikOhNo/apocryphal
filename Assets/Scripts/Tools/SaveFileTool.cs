using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SaveFileTool : EditorWindow
{
    string saveName = "MySave";
    Dictionary<string, int> cardCounts = new();
    Vector2 scroll;

    [MenuItem("Tools/Save Manager Tool")]
    public static void ShowWindow()
    {
        GetWindow<SaveFileTool>("Save Tool");
    }

    private void OnEnable()
    {
        LoadCards();
    }

    private void LoadCards()
    {
        cardCounts.Clear();
        Card[] allCards = Resources.LoadAll<Card>("");
        foreach (Card card in allCards)
        {
            cardCounts[card.name] = 0;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Save File Creator", EditorStyles.boldLabel);

        saveName = EditorGUILayout.TextField("Save Name", saveName);

        if (GUILayout.Button("Reload Cards"))
        {
            LoadCards();
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (var kvp in cardCounts.ToList())
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(kvp.Key);
            cardCounts[kvp.Key] = EditorGUILayout.IntField(cardCounts[kvp.Key]);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Create New Save"))
        {
            var newSave = SaveManager.CreateNewSave(saveName);
            newSave.deckCardCounts = cardCounts;
            SaveManager.UpdateSave(newSave, false);
            Debug.Log($"Created save: {newSave.saveName}");
        }

        if (GUILayout.Button("Load All Saves"))
        {
            var saves = SaveManager.LoadSaves();
            foreach (var save in saves)
            {
                Debug.Log($"Loaded: {save.saveName}");
            }
        }

        if (GUILayout.Button("Delete Save"))
        {
            SaveManager.DeleteSave(saveName);
        }
    }
}
