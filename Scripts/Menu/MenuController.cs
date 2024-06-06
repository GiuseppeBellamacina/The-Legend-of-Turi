using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [Header("Input")]
    public InputType inputType = InputType.Mouse;
    [Header("Event System")]
    public GameObject eventSystemObj;
    private bool eventSys;
    [Header("Save Popup")]
    public GameObject savePopup;
    [Header("PlayerUI")]
    public GameObject playerUI;
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
    [Header("Disclaimer Menu")]
    public GameObject disclaimerMenu;
    public GameObject disclaimerFirstSelectedButton;
    public GameObject disclaimerButton;
    public GameObject buttonGroup;
    private bool disclaimerOpen;

    // Input Actions
    Action<InputAction.CallbackContext> reselectAction;
    Action<InputAction.CallbackContext> backAction;
    Action<InputAction.CallbackContext> menuInteractionAction;

    void Awake()
    {
        FindEventSystem();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start()
    {
        menuInteractionAction = ctx => MenuInteraction();
        reselectAction = ctx => Reselect();
        backAction = ctx => CloseMenu();

        InputManager.Instance.inputController.Menu.MenuInteraction.performed += menuInteractionAction;
        InputManager.Instance.inputController.UI.ReSelect.performed += reselectAction;
        InputManager.Instance.inputController.UI.Back.performed += backAction;

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        disclaimerMenu.SetActive(false);
        savePopup.SetActive(false);

        lastSelectedButton = firstSelectedButton;
        InputManager.Instance.inputController.UI.Disable();
    }

    void FindEventSystem()
    {
        if (eventSystemObj == null)
        {
            eventSystemObj = EventSystem.current.gameObject;
            eventSys = EventSystem.current.enabled;
        }
    }

    public void MenuInteraction()
    {
        FindEventSystem();
        
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;

        if (pauseOpen)
        {
            InputManager.Instance.inputController.UI.Disable();
            InputManager.Instance.inputController.Player.Enable();
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            disclaimerMenu.SetActive(false);
            pauseOpen = false;
            settingsOpen = false;
            disclaimerOpen = false;
            playerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            playerUI.SetActive(false);
            InputManager.Instance.inputController.Player.Disable();
            InputManager.Instance.inputController.UI.Enable();
            pauseMenu.SetActive(true);
            pauseOpen = true;
            lastSelectedButton = firstSelectedButton;
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    void Reselect() // Per riprendere il controllo con il controller o la tastiera
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            inputType = InputType.Controller;
            if (disclaimerOpen && lastSelectedButton.transform.parent != disclaimerMenu.transform)
                EventSystem.current.SetSelectedGameObject(disclaimerFirstSelectedButton);
            else if (settingsOpen && lastSelectedButton.transform.parent != settingsMenu.transform)
                EventSystem.current.SetSelectedGameObject(settingsFirstSelectedButton);
            else
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

    public void Save()
    {
        DataManager.Instance.SaveData();
        StopAllCoroutines();
        savePopup.SetActive(true);
        savePopup.GetComponentInChildren<TextMeshProUGUI>().text = "Dati di gioco salvati correttamente";
        lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventSystem.current.SetSelectedGameObject(null);
        InputManager.Instance.inputController.UI.Disable();
        InputManager.Instance.inputController.Menu.Disable();
        EventSystem.current.enabled = false;
        eventSys = false;
        StartCoroutine(CloseSavePopup());
    }

    IEnumerator CloseSavePopup()
    {
        yield return new WaitForSecondsRealtime(2);
        savePopup.SetActive(false);
        InputManager.Instance.inputController.UI.Enable();
        InputManager.Instance.inputController.Menu.Enable();
        eventSystemObj.GetComponent<EventSystem>().enabled = true;
        eventSys = true;
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseMenu()
    {
        if (disclaimerOpen)
        {
            disclaimerMenu.SetActive(false);
            buttonGroup.SetActive(true);
            EventSystem.current.SetSelectedGameObject(disclaimerButton);
            disclaimerOpen = false;
            pauseOpen = true;

        }
        else if (settingsOpen)
        {
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsButton);
            settingsOpen = false;
        }
        else if (pauseOpen)
        {
            Time.timeScale = 1;
            InputManager.Instance.inputController.UI.Disable();
            InputManager.Instance.inputController.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            pauseOpen = false;
            playerUI.SetActive(true);
        }
    }

    public void OpenDisclaimer()
    {
        buttonGroup.SetActive(false);
        disclaimerMenu.SetActive(true);
        disclaimerOpen = true;
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(disclaimerFirstSelectedButton);
        else
            lastSelectedButton = disclaimerFirstSelectedButton;
    }

    public void SaveAndQuit()
    {
        Save();
        StartCoroutine(WaitAndQuit(savePopup));
    }

    IEnumerator WaitAndQuit(GameObject ogj)
    {
        while (ogj.activeSelf)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        QuitGame();
    }

    void Update()
    {
        if (pauseOpen && eventSys)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                lastSelectedButton = EventSystem.current.currentSelectedGameObject;
            CursorMoved();
        }
    }
}