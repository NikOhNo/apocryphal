using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    /*
     * class for managing encounters (aka opening and closing them for the most part)
     * EVERY encounter (be it combats, events, shops, etc.) should have a gameobject with this component on it somewhere
     */
    
    public event Action<Encounter> OnEncounterStart;
    public event Action<Encounter> OnEncounterEnd;
    
    // FIXME temporary measure for closing scene. just when you press the key specified in the closeSceneAction
    InputAction closeSceneAction;

    void Awake()
    {
        closeSceneAction = InputSystem.actions.FindAction("Cancel");
    }

    void OnEnable()
    {
        closeSceneAction.Enable();
        closeSceneAction.performed += OnPressCancel;
    }

    void OnDisable()
    {
        closeSceneAction.Disable();
        closeSceneAction.performed -= OnPressCancel;
    }

    void OnPressCancel(InputAction.CallbackContext context)
    {
        FinishEncounter();
    }

    
    public void FinishEncounter()
    {
        OnEncounterEnd?.Invoke(this);
    }
}
