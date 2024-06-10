using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FadeType { NullFade, FromColor, ToColor, FromBlack, ToBlack , NeroVerde, Final}

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public GameObject[] fadePanels;
    public float fadeWait;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
                if (_instance == null)
                    Debug.LogError("No LevelManager found in the scene.");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public IEnumerator FadeCo(string sceneName, bool willBeBounded)
    {
        // Disabilito l'input così il giocatore non può muoversi durante il cambio di scena
        InputManager.Instance.DisableInput();
        // Avvio il fade out
        AudioManager.Instance.FadeVolume(0.5f);
        Instantiate(fadePanels[(int)FadeType.ToColor], Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);
        // Setto la posizione del giocatore
        PlayerController.Instance.transform.position = GameController.Instance.startingPosition.value;
        // Carico la scena
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
            yield return null;
        // Setto la posizione della camera
        CameraMovement.Instance.isBounded = willBeBounded;
        CameraMovement.Instance.SetInstantPosition();
        // Avvio il fade in
        AudioManager.Instance.FadeVolume(0.5f, AudioManager.Instance.data.currentMasterVolume);
        GameObject panel = Instantiate(fadePanels[(int)FadeType.FromColor], Vector3.zero, Quaternion.identity);
        Destroy(panel, 1);
        // Riabilito l'input
        InputManager.Instance.EnableInput();
        CanvasSingleton.Instance.transform.Find("Menu").GetComponent<MenuController>().AssignActions();
        PlayerController.Instance.AssignActions();
    }

    public IEnumerator InitialFadeCo(string sceneName, bool willBeBounded, FadeType fadeIn, FadeType fadeOut, bool load = false, Vector2 position = default(Vector2))
    {
        InputManager.Instance.DisableInput();
        AudioManager.Instance.FadeVolume(0.5f);
        if (fadeOut != FadeType.NullFade)
            Instantiate(fadePanels[(int)fadeOut], Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // Impedisce l'attivazione della scena fino a quando non è esplicitamente permesso

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f) // Controllo il progresso del caricamento
            {
                yield return null;
                asyncOperation.allowSceneActivation = true;
                yield return null;

                AudioManager.Instance.FadeVolume(0.5f, AudioManager.Instance.data.currentMasterVolume);
                if (fadeIn != FadeType.NullFade)
                {
                    GameObject panel = Instantiate(fadePanels[(int)fadeIn], Vector3.zero, Quaternion.identity);
                    Destroy(panel, 1);
                }

                InputManager.Instance.EnableInput();

                break;
            }
            yield return null;
        }

        if (load)
        {
            StartCoroutine(WaitAndSet(position, willBeBounded));
        }
    }

    IEnumerator WaitAndSet(Vector2 position, bool willBeBounded)
    {
        while (PlayerController.Instance == null)
            yield return null;
        PlayerController.Instance.transform.position = position;

        while (CameraMovement.Instance == null)
            yield return null;
        CameraMovement.Instance.isBounded = willBeBounded;
        CameraMovement.Instance.SetInstantPosition();
    }

    public IEnumerator FinalFadeCo()
    {
        CanvasSingleton.Instance.transform.Find("Menu").GetComponent<MenuController>().RemoveActions();
        PlayerController.Instance.RemoveActions();
        InputManager.Instance.DisableInput();
        AudioManager.Instance.FadeVolume(0.5f);
        Instantiate(fadePanels[(int)FadeType.Final], Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Credits");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                DeleteGameManagers();
                
                yield return null;
                asyncOperation.allowSceneActivation = true;
                yield return null;

                AudioManager.Instance.FadeVolume(0.5f, AudioManager.Instance.data.currentMasterVolume);
                GameObject panel = Instantiate(fadePanels[(int)FadeType.FromBlack], Vector3.zero, Quaternion.identity);
                Destroy(panel, 1);

                break;
            }
            yield return null;
        }
    }

    public void DeleteGameManagers()
    {
        Destroy(GameController.Instance.gameObject);
        Destroy(RoomLocator.Instance.gameObject);
        Destroy(CameraMovement.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(CanvasSingleton.Instance.gameObject);
        Destroy(RespawnManager.Instance.gameObject);
        Destroy(DataManager.Instance.gameObject);
    }

    public void RespawnPlayer(bool withData)
    {
        if (withData)
        {
            StartCoroutine(InitialFadeCo(DataManager.Instance.gameStatus.currentScene, true, FadeType.FromColor, FadeType.NeroVerde, true, DataManager.Instance.gameStatus.playerPosition));
        }
        else
        {
            StartCoroutine(InitialFadeCo("Regno di Librino", true, FadeType.FromColor, FadeType.NeroVerde, true, GameController.Instance.startingPositionAbsolute));
        }
    }

    public void MenuStart()
    {
        // Questa serve ad avviare il gioco da zero dal menu principale
        StartCoroutine(InitialFadeCo("Intro", false, FadeType.NullFade, FadeType.ToBlack));
    }

    public void LoadGame(GameStatus gameStatus)
    {
        // Questa serva a caricare il gioco da un salvataggio
        string sceneName = gameStatus.currentScene;
        bool willBeBounded = gameStatus.isBounded;
        Vector2 position = new Vector2(gameStatus.playerPosition[0], gameStatus.playerPosition[1]);
        StartCoroutine(InitialFadeCo(sceneName, willBeBounded, FadeType.FromColor, FadeType.ToColor, true, position));
    }

    public void MainMenuScene()
    {
        // Questa serve a tornare al menu principale
        StartCoroutine(InitialFadeCo("MainMenu", false, FadeType.FromColor, FadeType.ToColor));
    }

    public void StartGame()
    {
        // Questa serve a iniziare il gioco dal video introduttivo
        StartCoroutine(InitialFadeCo("Regno di Librino", true, FadeType.FromColor, FadeType.NeroVerde));
    }

    public void CreditsScene()
    {
        // Questa serve a vedere i crediti
        StartCoroutine(FinalFadeCo());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}