using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public GameObject eventSystemObj;
    public GameObject errorPopup;
    public GameObject firstSelectedButton;
    public GameObject lastSelectedButton;
    bool eventSys;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        errorPopup.SetActive(false);
        eventSys = EventSystem.current.enabled;
    }

    void Start()
    {
        InputManager.Instance.inputController.UI.ReSelect.performed += ctx => Reselect();
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void NewGame()
    {
        DataManager.Instance.DeleteData();
        DataManager.Instance.ResetIndexes();
        DataManager.Instance.InitializeIndexes();
        LevelManager.Instance.MenuStart();
    }

    public void LoadGame(GameStatus gameStatus)
    {
        if (!DataManager.Instance.LoadData())
        {
            StopAllCoroutines();
            errorPopup.SetActive(true);
            errorPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Errore:\nnessun salvataggio trovato.";
            InputManager.Instance.inputController.UI.Disable();
            EventSystem.current.enabled = false;
            eventSys = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Reselect()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (eventSys)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }
}