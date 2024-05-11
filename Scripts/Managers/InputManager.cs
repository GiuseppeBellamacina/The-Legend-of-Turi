using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public InputController inputController;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                    Debug.LogError("No InputManager found in the scene.");
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

        inputController = new InputController();
        inputController.Enable();
    }
}
