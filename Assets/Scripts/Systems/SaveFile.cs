using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[System.Serializable]
public class SaveFile 
{
    //-- Save File Data (metadata)
    public string saveName;
    public string saveDate;
    public string saveTime;
    public float playTime;

    //-- Scene Data
    public string lastScene;

    //-- Player Data

    //-- Helper Functions
    public void UpdateSaveMetadata()
    {
        saveDate = DateTime.Now.ToString("yyyy-MM-dd");
        saveTime = DateTime.Now.ToString("HH:mm:ss");

        //TODO: record playtime
    }
}
