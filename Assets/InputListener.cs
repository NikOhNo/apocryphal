using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputListener : MonoBehaviour
{
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
        var currentScene = gameObject.scene;
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
