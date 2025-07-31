using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterData", menuName = "Scriptable Objects/EncounterData")]
public class EncounterData : ScriptableObject
{
    public List<Enemy> enemies; // ensure this is a list of enemy prefabs
}
