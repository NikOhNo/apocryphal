using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapEncounterManager : MonoBehaviour
{
    /*
     * encounter manager singleton! for controlling all encounters in the game!
     * specifically managers encounters on the map side, rather than on the encounter side (meaning this object will be in the Map scene rather than on every encounter)
     * encounters are:
     *      combat
     *      events
     *      shops
     *      etc.
     * basically things that interrupt the mapview of the game
     * all of these things will be loaded in over top of the map view
     * while also pausing it
     */
    
    public static MapEncounterManager Instance { get; private set; }
    
    private Encounter currentEncounter;
    
    private MapManager mapManager; // ref to mapmanager... may create a bit of a circular dependency but it shouldn't be that big of a deal
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
            if (mapManager == null)
            {
                Debug.LogError("MapManager not found, consider adding one to make the encountermanager work");
            }
        }
    }
    
    
    public void StartNewEncounter(SceneAsset encounterScene)
    {
        string path = AssetDatabase.GetAssetOrScenePath(encounterScene);
        SceneManager.LoadSceneAsync(SceneUtility.GetBuildIndexByScenePath(path), LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnEncounterLoaded;
        
        // do other stuff...... :3 perhaps
    }

    private void OnEncounterLoaded(Scene scn, LoadSceneMode mode)
    {
        
        var gos = scn.GetRootGameObjects();
        foreach (var go in gos)
        {
            if (go.TryGetComponent<Encounter>(out Encounter encounter))
            {
                currentEncounter = encounter;
                encounter.OnEncounterEnd += OnEncounterEnd;
            }
        }
        
        mapManager?.OnEncounterStart();
    }

    private void OnEncounterEnd(Encounter e)
    {
        Debug.Log("Encounter ended!");
        // destroy the scene
        SceneManager.UnloadSceneAsync(currentEncounter.gameObject.scene);
        mapManager?.OnEncounterEnd();
    }
}
