using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager _instance;

    public static ScreenManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScreenManager>();
                if (_instance == null)
                    Debug.LogError("No ScreenManager found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
