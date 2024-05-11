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

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator FadeCo(string sceneName, bool willBeBounded){
        if (fadeOutPanel != null)
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);
        PlayerController.Instance.transform.position = GameController.Instance.startingPosition.value;
        CameraMovement.Instance.isBounded = willBeBounded;
        CameraMovement.Instance.SetInstantPosition();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
            yield return null;
        //ObjectManager.Instance.ResetChests();
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}