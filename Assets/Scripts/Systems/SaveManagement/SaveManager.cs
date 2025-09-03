using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static string SavePath => Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    public static JsonSerializerSettings Settings => new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto, // Handles polymorphic deserialization
        Formatting = Formatting.Indented,
    };

    public static SaveFile CreateNewSave(string saveName)
    {
        SaveFile saveFile = new();
        saveFile.saveName = saveName;
        saveFile.lastScene = "SampleScene";

        UpdateSave(saveFile, false);
        return saveFile;
    }

    public static void UpdateSave(SaveFile saveFile, bool recordScene)
    {
        saveFile.UpdateSaveMetadata();
        if (recordScene) saveFile.lastScene = SceneManager.GetActiveScene().name;

        string saveJson = JsonConvert.SerializeObject(saveFile, Settings);

        File.WriteAllText(SavePath + saveFile.saveName + ".json", saveJson);
    }

    public static List<SaveFile> LoadSaves()
    {
        List<SaveFile> saveFiles = new();
        string[] saveFilePaths = Directory.GetFiles(SavePath, "*.json");

        foreach (string saveFilePath in saveFilePaths)
        {
            string saveJson = File.ReadAllText(saveFilePath);
            SaveFile saveFile = JsonConvert.DeserializeObject<SaveFile>(saveJson, Settings);

            if (saveFile != null)
            {
                saveFiles.Add(saveFile);
            }
        }

        return saveFiles;
    }

    public static void DeleteSave(SaveFile saveFile)
    {
        DeleteSave(saveFile.saveName);
    }

    public static void DeleteSave(string saveName)
    {
        string filePath = SavePath + saveName + ".json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Save file at path \"{filePath}\" deleted.");
        }
        else
        {
            Debug.Log($"Save file at path \"{filePath}\" not found. Could not delete.");
        }
    }
}
