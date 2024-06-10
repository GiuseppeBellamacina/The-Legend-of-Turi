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

    // Input Actions
    Action<InputAction.CallbackContext> reselectAction;
    Action<InputAction.CallbackContext> cancelAction;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        errorPopup.SetActive(false);
        eventSys = EventSystem.current.enabled;
    }

    void Start()
    {
        reselectAction = ctx => Reselect();
        cancelAction = ctx => CloseMenu();

        InputManager.Instance.inputController.UI.ReSelect.performed += reselectAction;
        InputManager.Instance.inputController.UI.Cancel.performed += cancelAction;
        
        lastSelectedButton = firstSelectedButton;
    }

    public void NewGame()
    {
        DataManager.Instance.DeleteData();
        DataManager.Instance.Reset();
        DataManager.Instance.ResetIndexes();
        DataManager.Instance.InitializeIndexes();

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
        DataManager.Instance.gameStatus.difficulty = difficulty;

        InputManager.Instance.inputController.UI.ReSelect.performed -= reselectAction;
        InputManager.Instance.inputController.UI.Cancel.performed -= cancelAction;
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

    void Reselect() // Per riprendere il controllo con il controller o la tastiera
    {
        inputType = InputType.Controller;
        if (optionsOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, optionsMenu, 3))
            EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
        else if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            if (optionsOpen && EventSystem.current.currentSelectedGameObject != null &&  !CheckParent(lastSelectedButton, optionsMenu, 3))
                EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
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
            optionsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(optionsButton);
            optionsOpen = false;
        }
        else if (difficultyOpen)
        {
            difficultyMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(newGameButton);
            difficultyOpen = false;
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