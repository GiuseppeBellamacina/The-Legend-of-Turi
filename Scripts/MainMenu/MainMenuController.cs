using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public enum InputType
{
    Mouse,
    Controller
}

public class MainMenuController : MonoBehaviour
{
    // Input type
    public InputType inputType = InputType.Mouse;
    // EventSystem
    public GameObject eventSystemObj;
    private bool eventSys;
    // Error popup
    public GameObject errorPopup;
    // Fade panels
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    // Selected buttons
    public GameObject firstSelectedButton;
    public GameObject lastSelectedButton;

    // Options menu
    public GameObject optionsMenu;
    public GameObject optionsFirstSelectedButton;
    public GameObject optionsButton;
    private bool optionsOpen;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        errorPopup.SetActive(false);
        eventSys = EventSystem.current.enabled;
        InputManager.Instance.inputController.UI.Disable();
    }

    void Start()
    {
        InputManager.Instance.inputController.UI.ReSelect.performed += ctx => Reselect();
        InputManager.Instance.inputController.UI.Cancel.performed += ctx => CloseOptionsMenu();
        lastSelectedButton = firstSelectedButton;

        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            StartCoroutine(ActivateFirstInput(panel));
            Destroy(panel, 1);
        }
    }

    IEnumerator ActivateFirstInput(GameObject panel)
    {
        while (panel != null)
            yield return null;
        InputManager.Instance.inputController.UI.Enable();
    }

    public void NewGame()
    {
        DataManager.Instance.DeleteData();
        DataManager.Instance.ResetIndexes();
        DataManager.Instance.InitializeIndexes();
        LevelManager.Instance.MenuStart(fadeOutPanel);
    }

    public void LoadGame(GameStatus gameStatus)
    {
        if (!DataManager.Instance.LoadData())
        {
            StopAllCoroutines();
            errorPopup.SetActive(true);
            errorPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Errore:\nnessun salvataggio trovato.";
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
            LevelManager.Instance.MenuStart(gameStatus);
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
        optionsOpen = true;
        optionsMenu.SetActive(true);
        if (inputType == InputType.Controller)
            EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
        else
            lastSelectedButton = optionsFirstSelectedButton;
    }

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        if (optionsOpen)
            EventSystem.current.SetSelectedGameObject(optionsButton);
        optionsOpen = false;
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