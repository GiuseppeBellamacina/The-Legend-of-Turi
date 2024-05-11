using UnityEngine;

public class RoomLocator : MonoBehaviour
{
    public GameObject minPositionObject, maxPositionObject;

    private static RoomLocator _instance;

    public static RoomLocator instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RoomLocator>();
                if (_instance == null)
                    Debug.LogError("No RoomLocator found in the scene.");
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

    void Start()
    {
        CameraMovement.Instance.SetMinMaxPositionObjects(minPositionObject, maxPositionObject);
    }

    public void SetMinMaxPositionObjects(GameObject minPosObject, GameObject maxPosObject)
    {
        minPositionObject = minPosObject;
        maxPositionObject = maxPosObject;
        CameraMovement.Instance.SetMinMaxPositionObjects(minPositionObject, maxPositionObject);
    }
}
