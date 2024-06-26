using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEditor;

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
    [Header("Tutorial Menu")]
    public BoolValue tutorialDone;
    public GameObject tutorialMenu;
    public GameObject tutorialFirstSelectedButton;
    public GameObject tutorialButton;
    private bool tutorialOpen;
    [Header("Audio")]
    public AudioClip buttonSound;
    public AudioClip selectSound;
    public AudioClip backSound;
    public AudioClip openSound;

    // Input Actions
    Action<InputAction.CallbackContext> reselectAction;
    Action<InputAction.CallbackContext> backAction;
    Action<InputAction.CallbackContext> menuInteractionAction;
    Action<InputAction.CallbackContext> submitAction;

    public void Awake()
    {
        FindEventSystem();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AssignActions();

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        disclaimerMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        savePopup.SetActive(false);

        pauseOpen = false;
        settingsOpen = false;
        disclaimerOpen = false;
        tutorialOpen = false;

        lastSelectedButton = firstSelectedButton;
        InputManager.Instance.inputController.UI.Disable();
        Tutorial();
    }

    public void RemoveActions()
    {
        InputManager.Instance.inputController.Menu.MenuInteraction.performed -= menuInteractionAction;
        InputManager.Instance.inputController.UI.ReSelect.performed -= reselectAction;
        InputManager.Instance.inputController.UI.Back.performed -= backAction;
        InputManager.Instance.inputController.UI.Submit.performed -= submitAction;

        InputManager.Instance.inputController.UI.Disable();
        InputManager.Instance.inputController.Menu.Disable();
    }

    public void AssignActions()
    {
        menuInteractionAction = ctx => MenuInteraction();
        reselectAction = ctx => Reselect(ctx);
        backAction = ctx => CloseMenu();
        submitAction = ctx => SelectSound();

        InputManager.Instance.inputController.Menu.MenuInteraction.performed += menuInteractionAction;
        InputManager.Instance.inputController.UI.ReSelect.performed += reselectAction;
        InputManager.Instance.inputController.UI.Back.performed += backAction;
        InputManager.Instance.inputController.UI.Submit.performed += submitAction;

        InputManager.Instance.inputController.UI.Enable();
        InputManager.Instance.inputController.Menu.Enable();
    }

    void Tutorial()
    {
        if (!tutorialDone.value)
        {
            InputManager.Instance.inputController.Player.Disable();
            playerUI.SetActive(false);
            pauseMenu.SetActive(true);
            tutorialMenu.SetActive(true);
            tutorialOpen = true;
            pauseOpen = true;
            EventSystem.current.SetSelectedGameObject(tutorialFirstSelectedButton);
            InputManager.Instance.inputController.UI.Enable();
        }
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
        
        // Chiudo il menu se è aperto
        if (pauseOpen)
        {
            AudioManager.Instance.sfxSource.PlayOneShot(backSound);
            AudioManager.Instance.IncreaseMusic(0.5f);
            InputManager.Instance.inputController.UI.Disable();
            Time.timeScale = 1;
            if (pauseMenu != null)
                pauseMenu.SetActive(false);
            if (settingsMenu != null)
                settingsMenu.SetActive(false);
            if (disclaimerMenu != null)
                disclaimerMenu.SetActive(false);
            if (tutorialMenu != null)
                tutorialMenu.SetActive(false);
            pauseOpen = false;
            settingsOpen = false;
            disclaimerOpen = false;
            tutorialOpen = false;
            if (playerUI != null)
                playerUI.SetActive(true);
            InputManager.Instance.inputController.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // Apro il menu se è chiuso
        else
        {
            AudioManager.Instance.sfxSource.PlayOneShot(openSound);
            AudioManager.Instance.DecreaseMusic(0.5f);
            InputManager.Instance.inputController.Player.Disable();
            Time.timeScale = 0;
            if (playerUI != null)
                playerUI.SetActive(false);
            if (pauseMenu != null)
                pauseMenu.SetActive(true);
            pauseOpen = true;
            lastSelectedButton = firstSelectedButton;
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            InputManager.Instance.inputController.UI.Enable();
        }
    }

    void Reselect(InputAction.CallbackContext context) // Per riprendere il controllo con il controller o la tastiera
    {
        if (!pauseOpen || context.ReadValue<Vector2>() == Vector2.zero)
            return;

        if (!AudioManager.Instance.sfxSource.isPlaying)
            OverSound();
        inputType = InputType.Controller;
        if (disclaimerOpen && EventSystem.current.currentSelectedGameObject != null && !CheckParent(lastSelectedButton, disclaimerMenu, 3))
            EventSystem.current.SetSelectedGameObject(disclaimerFirstSelectedButton);
        else if (settingsOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, settingsMenu, 3))
            EventSystem.current.SetSelectedGameObject(settingsFirstSelectedButton);
        else if (tutorialOpen && EventSystem.current.currentSelectedGameObject != null && !CheckParent(lastSelectedButton, tutorialMenu, 3))
            EventSystem.current.SetSelectedGameObject(tutorialFirstSelectedButton);
        else if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            if (disclaimerOpen && EventSystem.current.currentSelectedGameObject != null && !CheckParent(lastSelectedButton, disclaimerMenu, 3))
                EventSystem.current.SetSelectedGameObject(disclaimerFirstSelectedButton);
            else if (settingsOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, settingsMenu, 3))
                EventSystem.current.SetSelectedGameObject(settingsFirstSelectedButton);
            else if (tutorialOpen && EventSystem.current.currentSelectedGameObject != null && !CheckParent(lastSelectedButton, tutorialMenu, 3))
                EventSystem.current.SetSelectedGameObject(tutorialFirstSelectedButton);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    bool CheckParent(GameObject obj, GameObject parent, int depth)
    {
        if (depth == 0)
            return false;
        if (obj.transform.parent == parent.transform)
            return true;
        if (obj.transform.parent == null)
            return false;
        else
            return CheckParent(obj.transform.parent.gameObject, parent, depth - 1);
    }

    void CursorMoved() // Per far apparire il cursore quando si muove il mouse
    {
        if (!pauseOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
        
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
        if (!tutorialDone.value)
        {
            SelectSound();
            MenuInteraction();
            tutorialDone.value = true;
            return;
        }

        if (disclaimerOpen)
        {
            SelectSound();
            disclaimerMenu.SetActive(false);
            buttonGroup.SetActive(true);
            EventSystem.current.SetSelectedGameObject(disclaimerButton);
            disclaimerOpen = false;
            pauseOpen = true;

        }
        else if (settingsOpen)
        {
            SelectSound();
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsButton);
            settingsOpen = false;
        }
        else if (tutorialOpen)
        {
            SelectSound();
            tutorialMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(tutorialButton);
            tutorialOpen = false;
        }
        else if (pauseOpen)
        {
            SelectSound();
            AudioManager.Instance.sfxSource.PlayOneShot(backSound);
            AudioManager.Instance.IncreaseMusic(0.5f);
            InputManager.Instance.inputController.UI.Disable();
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            pauseOpen = false;
            playerUI.SetActive(true);
            InputManager.Instance.inputController.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

    public void OpenTutorial()
    {
        tutorialMenu.SetActive(true);
        tutorialOpen = true;
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(tutorialFirstSelectedButton);
        else
            lastSelectedButton = tutorialFirstSelectedButton;
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

    public void OverSound()
    {
        if (AudioManager.Instance.sfxSource.isPlaying)
            AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.sfxSource.PlayOneShot(buttonSound);
    }

    public void SelectSound()
    {
        if (AudioManager.Instance.sfxSource.isPlaying)
            AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.sfxSource.PlayOneShot(selectSound);
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