using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public InputController inputController;
    private static bool _isInitialized;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                {
                    Debug.LogError("No InputManager found in the scene. Please ensure there is one InputManager in the initial scene.");
                }
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
            InitializeInputController();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInputController()
    {
        if (!_isInitialized)
        {
            inputController = new InputController();
            inputController.Enable();
            _isInitialized = true;
        }
    }
}
