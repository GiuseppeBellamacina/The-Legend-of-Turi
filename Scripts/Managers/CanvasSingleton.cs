using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    private static CanvasSingleton _instance;
    public GameObject[] toDeactivate;

    public static CanvasSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CanvasSingleton>();
                if (_instance == null)
                {
                    Debug.LogError("Canvas not found");
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (GameObject obj in toDeactivate)
        {
            obj.SetActive(false);
        }
    }
}