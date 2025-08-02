using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public string SavePath => Application.persistentDataPath + Path.AltDirectorySeparatorChar;

    public SaveFile CreateNewSave(string saveName)
    {
        SaveFile saveFile = new();
        saveFile.saveName = saveName;
        saveFile.lastScene = "SampleScene";

        UpdateSave(saveFile, false);
        return saveFile;
    }

    public void UpdateSave(SaveFile saveFile, bool recordScene)
    {
        saveFile.UpdateSaveMetadata();
        if (recordScene) saveFile.lastScene = SceneManager.GetActiveScene().name;

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto, // Handles polymorphic deserialization
            Formatting = Formatting.Indented
        };

        string saveJson = JsonConvert.SerializeObject(saveFile, settings);
        File.WriteAllText(SavePath + saveFile.saveName + ".json", saveJson);
    }

    public List<SaveFile> LoadSaves()
    {
        List<SaveFile> saveFiles = new();
        string[] saveFilePaths = Directory.GetFiles(SavePath, "*.json");

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto // Handles polymorphic deserialization
        };

        foreach (string saveFilePath in saveFilePaths)
        {
            string saveJson = File.ReadAllText(saveFilePath);
            SaveFile saveFile = JsonConvert.DeserializeObject<SaveFile>(saveJson, settings);

            if (saveFile != null)
            {
                saveFiles.Add(saveFile);
            }
        }

        return saveFiles;
    }
}
