using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    [SerializeField]
    GameObject fadeInPanel, fadeOutPanel;
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
    }

    public IEnumerator MenuFadeCo(string sceneName, bool willBeBounded, Vector2 position, bool load){
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

    public void MenuStart()
    {
        StartCoroutine(MenuFadeCo("Regno di Librino", true, Vector2.zero, false));
    }

    public void MenuStart(GameStatus gameStatus)
    {
        string sceneName = gameStatus.currentScene;
        bool willBeBounded = gameStatus.isBounded;
        Vector2 position = new Vector2(gameStatus.playerPosition[0], gameStatus.playerPosition[1]);
        StartCoroutine(MenuFadeCo(sceneName, willBeBounded, position, true));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}