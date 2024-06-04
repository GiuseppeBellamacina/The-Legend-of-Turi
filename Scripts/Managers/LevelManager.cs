using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    [SerializeField]
    GameObject fadeInPanel, fadeOutPanel, startFade;
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

    public IEnumerator FadeCo(string sceneName, bool willBeBounded){
        InputManager.Instance.inputController.Disable();
        if (fadeOutPanel != null)
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);
        PlayerController.Instance.transform.position = GameController.Instance.startingPosition.value;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
            yield return null;
        CameraMovement.Instance.isBounded = willBeBounded;
        CameraMovement.Instance.SetInstantPosition();
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1);
        }
        InputManager.Instance.inputController.Enable();
    }

    public IEnumerator InitialFadeCo(string sceneName, bool willBeBounded, bool load = false, Vector2 position = default(Vector2), GameObject fadeInPanel = null, GameObject fadeOutPanel = null)
    {
        if (fadeOutPanel != null)
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
            yield return null;
        if (load)
        {
            PlayerController.Instance.transform.position = position;
            CameraMovement.Instance.isBounded = willBeBounded;
            CameraMovement.Instance.SetInstantPosition();
        }
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1);
        }
    }

    public void MenuStart(GameObject fadeOutPanel)
    {
        // Questa serve ad avviare il gioco da zero dal menu principale
        StartCoroutine(InitialFadeCo("Intro", true, false, Vector2.zero, null, fadeOutPanel));
    }

    public void MenuStart(GameStatus gameStatus)
    {
        // Questa serva a caricare il gioco da un salvataggio
        string sceneName = gameStatus.currentScene;
        bool willBeBounded = gameStatus.isBounded;
        Vector2 position = new Vector2(gameStatus.playerPosition[0], gameStatus.playerPosition[1]);
        StartCoroutine(InitialFadeCo(sceneName, willBeBounded, true, position));
    }

    public void MainMenuScene()
    {
        StartCoroutine(InitialFadeCo("MainMenu", false, false, Vector2.zero, startFade, null));
    }

    public void StartGameScene()
    {
        StartCoroutine(InitialFadeCo("Regno di Librino", true, false, Vector2.zero, startFade, null));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}