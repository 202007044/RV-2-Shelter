using UnityEngine;
using UnityEngine.InputSystem;

public class ClipboardController : MonoBehaviour
{
    public GameObject clipboardObject; // Assign your clipboard object in the Inspector
    public bool isInteractiveModeActive = false;
    public InputActionReference activateInteractiveMode;

    void Awake()
    {
        activateInteractiveMode.action.Enable();
        activateInteractiveMode.action.performed += ToggleInteractiveMode;
    }

    private void ToggleInteractiveMode(InputAction.CallbackContext context)
    {
        isInteractiveModeActive = !isInteractiveModeActive;
        clipboardObject.SetActive(isInteractiveModeActive); // Show or hide the clipboard
    }
}
