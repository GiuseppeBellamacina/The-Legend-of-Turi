using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem;

public enum InputType
{
    Mouse,
    Controller
}

public class MainMenuController : MonoBehaviour
{
    [Header("Input")]
    public InputType inputType = InputType.Mouse;
    [Header("Event System")]
    public GameObject eventSystemObj;
    private bool eventSys;
    [Header("Error Popup")]
    public GameObject errorPopup;
    [Header("Buttons")]
    public GameObject firstSelectedButton;
    public GameObject lastSelectedButton;

    [Header("Options")]
    public GameObject optionsMenu;
    public GameObject optionsFirstSelectedButton;
    public GameObject optionsButton;
    private bool optionsOpen;

    [Header("Difficulty")]
    public GameObject newGameButton;
    public GameObject difficultyMenu;
    public GameObject difficultyFirstSelectedButton;
    bool difficultyOpen;
    [Header("Audio")]
    public AudioClip buttonOver;
    public AudioClip buttonSelect;

    // Input Actions
    Action<InputAction.CallbackContext> reselectAction;
    Action<InputAction.CallbackContext> cancelAction;
    Action<InputAction.CallbackContext> submitAction;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        errorPopup.SetActive(false);
        eventSys = EventSystem.current.enabled;
    }

    void Start()
    {
        reselectAction = ctx => Reselect(ctx);
        cancelAction = ctx => CloseMenu();
        submitAction = ctx => SelectSound();

        InputManager.Instance.inputController.UI.ReSelect.performed += reselectAction;
        InputManager.Instance.inputController.UI.Cancel.performed += cancelAction;
        InputManager.Instance.inputController.UI.Submit.performed += submitAction;

        InputManager.Instance.inputController.UI.Enable();
        
        lastSelectedButton = firstSelectedButton;
    }

    public void NewGame()
    {
        SelectDifficulty();
    }

    void SelectDifficulty()
    {
        difficultyOpen = true;
        difficultyMenu.SetActive(true);
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(difficultyFirstSelectedButton);
        else
            lastSelectedButton = difficultyFirstSelectedButton;
    }

    public void SetDifficulty(int difficulty)
    {
        DataManager.Instance.DeleteData();
        DataManager.Instance.Reset();
        DataManager.Instance.ResetIndexes();
        DataManager.Instance.InitializeIndexes();
        
        DataManager.Instance.gameStatus.difficulty = difficulty;

        InputManager.Instance.inputController.UI.ReSelect.performed -= reselectAction;
        InputManager.Instance.inputController.UI.Cancel.performed -= cancelAction;
        InputManager.Instance.inputController.UI.Submit.performed -= submitAction;
        LevelManager.Instance.MenuStart();
    }

    public void LoadGame(GameStatus gameStatus)
    {
        DataManager.Instance.Reset();
        if (!DataManager.Instance.LoadData())
        {
            StopAllCoroutines();
            errorPopup.SetActive(true);
            errorPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Errore:\nnessun salvataggio trovato";
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EventSystem.current.SetSelectedGameObject(null);
            InputManager.Instance.inputController.UI.Disable();
            EventSystem.current.enabled = false;
            eventSys = false;
            StartCoroutine(CloseErrorPopup());
        }
        else
        {
            InputManager.Instance.inputController.UI.ReSelect.performed -= reselectAction;
            InputManager.Instance.inputController.UI.Cancel.performed -= cancelAction;
            InputManager.Instance.inputController.UI.Submit.performed -= submitAction;
            LevelManager.Instance.LoadGame(gameStatus);
        }
    }

    IEnumerator CloseErrorPopup()
    {
        yield return new WaitForSeconds(2);
        errorPopup.SetActive(false);
        InputManager.Instance.inputController.UI.Enable();
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

    void Reselect(InputAction.CallbackContext context) // Per riprendere il controllo con il controller o la tastiera
    {
        if (context.ReadValue<Vector2>() == Vector2.zero)
            return;

        if (!AudioManager.Instance.sfxSource.isPlaying)
            OverSound();
        inputType = InputType.Controller;
        if (optionsOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, optionsMenu, 3))
            EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
        else if (difficultyOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, difficultyMenu, 3))
            EventSystem.current.SetSelectedGameObject(difficultyFirstSelectedButton);
        else if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            if (optionsOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, optionsMenu, 3))
                EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
            else if (difficultyOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, difficultyMenu, 3))
                EventSystem.current.SetSelectedGameObject(difficultyFirstSelectedButton);
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
        optionsOpen = true;
        optionsMenu.SetActive(true);
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
        else
            lastSelectedButton = optionsFirstSelectedButton;
    }

    public void CloseMenu()
    {
        if (optionsOpen)
        {
            SelectSound();
            optionsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(optionsButton);
            optionsOpen = false;
        }
        else if (difficultyOpen)
        {
            SelectSound();
            difficultyMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(newGameButton);
            difficultyOpen = false;
        }
    }

    public void OverSound()
    {
        if (AudioManager.Instance.sfxSource.isPlaying)
            AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.sfxSource.PlayOneShot(buttonOver);
    }

    public void SelectSound()
    {
        if (AudioManager.Instance.sfxSource.isPlaying)
            AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.sfxSource.PlayOneShot(buttonSelect);
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