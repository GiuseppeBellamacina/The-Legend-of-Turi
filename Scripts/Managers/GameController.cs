using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public VectorValue startingPosition, lastPosition;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
                if (_instance == null)
                    Debug.LogError("No GameController found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null){
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        PlayerController.Instance.SetState(State.none);
        startingPosition.value = PlayerController.Instance.transform.position;
    }

    public bool PlayerUsingController()
    {
        return Input.GetJoystickNames().Length > 0;
    }

    public void LoadScene(string sceneName, bool willBeBounded)
    {
        lastPosition.value = PlayerController.Instance.transform.position;
        StartCoroutine(LevelManager.Instance.FadeCo(sceneName, willBeBounded));
        //lastPosition.value = PlayerController.Instance.transform.position;
        PlayerController.Instance.SetState(State.none);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
