using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Header("Input")]
    public InputType inputType = InputType.Mouse;
    [Header("Event System")]
    public GameObject eventSystemObj;
    private bool eventSys;
    [Header("Buttons")]
    public GameObject firstSelectedButton;
    public GameObject lastSelectedButton;
    [Header("Pause Menu")]
    public GameObject pauseMenu;
    private bool pauseOpen;
    [Header("Settings Menu")]
    public GameObject settingsMenu;
    public GameObject settingsFirstSelectedButton;
    public GameObject settingsButton;
    private bool settingsOpen;

    // Input Actions
    Action<InputAction.CallbackContext> reselectAction;
    Action<InputAction.CallbackContext> backAction;
    Action<InputAction.CallbackContext> menuInteractionAction;

    void Start()
    {
        eventSystemObj = EventSystem.current.gameObject;
        eventSys = EventSystem.current.enabled;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuInteractionAction = ctx => MenuInteraction();
        reselectAction = ctx => Reselect();
        backAction = ctx => CloseMenu();

        InputManager.Instance.inputController.Menu.MenuInteraction.performed += menuInteractionAction;
        InputManager.Instance.inputController.UI.ReSelect.performed += reselectAction;
        InputManager.Instance.inputController.UI.Back.performed += backAction;

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        lastSelectedButton = firstSelectedButton;
        InputManager.Instance.inputController.UI.Disable();
    }

    public void MenuInteraction()
    {
        if (pauseOpen)
        {
            InputManager.Instance.inputController.UI.Disable();
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            pauseOpen = false;
            settingsOpen = false;
        }
        else
        {
            InputManager.Instance.inputController.UI.Enable();
            pauseMenu.SetActive(true);
            pauseOpen = true;
        }
    }

    void Reselect() // Per riprendere il controllo con il controller o la tastiera
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            inputType = InputType.Controller;
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void CursorMoved() // Per far apparire il cursore quando si muove il mouse
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            inputType = InputType.Mouse;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void QuitGame()
    {
        LevelManager.Instance.QuitGame();
    }

    public void OpenOptionsMenu()
    {
        settingsOpen = true;
        settingsMenu.SetActive(true);
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(settingsFirstSelectedButton);
        else
            lastSelectedButton = settingsFirstSelectedButton;
    }

    public void CloseMenu()
    {
        if (settingsOpen)
        {
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsButton);
            settingsOpen = false;
        }
        else if (pauseOpen)
        {
            InputManager.Instance.inputController.UI.Disable();
            pauseMenu.SetActive(false);
            pauseOpen = false;
        }
    }

    void Update()
    {
        if (eventSys)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                lastSelectedButton = EventSystem.current.currentSelectedGameObject;
            CursorMoved();
        }
    }
}