using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public Vector2 startingPositionAbsolute;
    public VectorValue startingPosition, lastPosition;
    public FloatValue healthMultiplier, damageMultiplier;
    public GameStatus gameStatus;

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
        startingPositionAbsolute = startingPosition.value;
        SetMultipliers();
        Cursor.visible = false;
    }

    void SetMultipliers()
    {
        switch (gameStatus.difficulty)
        {
            case 0:
                healthMultiplier.value = 0.75f;
                damageMultiplier.value = 1.25f;
                break;
            case 1:
                healthMultiplier.value = 1f;
                damageMultiplier.value = 1f;
                break;
            case 2:
                healthMultiplier.value = 1.5f;
                damageMultiplier.value = 0.75f;
                break;
        }
    }

    public bool PlayerUsingController()
    {
        return Input.GetJoystickNames().Length > 0;
    }

    public void LoadScene(string sceneName, bool willBeBounded)
    {
        lastPosition.value = PlayerController.Instance.transform.position;
        CanvasSingleton.Instance.transform.Find("Menu").GetComponent<MenuController>().RemoveActions();
        PlayerController.Instance.RemoveActions();
        StartCoroutine(LevelManager.Instance.FadeCo(sceneName, willBeBounded));
        PlayerController.Instance.SetState(State.none);
    }
}
