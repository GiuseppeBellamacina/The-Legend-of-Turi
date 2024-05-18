using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    private static bool _isInitialized;
    public Manageable[] toReset;
    // public ScriptableObject[] toSave;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataManager>();
                if (_instance == null)
                    Debug.LogError("No DataManager found in the scene.");
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
        
        if (!_isInitialized)
        {
            _isInitialized = true;
            Reset();
        }
    }

    public void Reset()
    {
        foreach (Manageable script in toReset)
        {
            script.Reset();
        }
    }

    public void Save()
    {
        // Save the data
    }
}