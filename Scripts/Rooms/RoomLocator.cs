using UnityEngine;

public class RoomLocator : MonoBehaviour
{
    private GameObject minPositionObject, maxPositionObject;
    public GameObject currentRoom;

    private static RoomLocator _instance;

    public static RoomLocator Instance
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
        
        FindPositionObjects();
        CameraMovement.Instance.SetMinMaxPositionObjects(minPositionObject, maxPositionObject);
    }

    void Start()
    {
        ActivatecurrentRoom();
    }

    public void FindPositionObjects()
    {
        GameObject[] minPositionObjects, maxPositionObjects;
        minPositionObjects = GameObject.FindGameObjectsWithTag("MinPosition");
        maxPositionObjects = GameObject.FindGameObjectsWithTag("MaxPosition");

        float minDistance = Mathf.Infinity;

        foreach (GameObject minPosObject in minPositionObjects)
        {
            float distance = Vector2.Distance(PlayerController.Instance.transform.position, minPosObject.transform.position);
            if (distance < minDistance)
            {
                if (PlayerController.Instance.transform.position.x >= minPosObject.transform.position.x && PlayerController.Instance.transform.position.y >= minPosObject.transform.position.y)
                {
                    minDistance = distance;
                    minPositionObject = minPosObject;
                }
            }    
        }

        minDistance = Mathf.Infinity;

        foreach (GameObject maxPosObject in maxPositionObjects)
        {
            float distance = Vector2.Distance(PlayerController.Instance.transform.position, maxPosObject.transform.position);
            if (distance < minDistance)
            {
                if (PlayerController.Instance.transform.position.x <= maxPosObject.transform.position.x && PlayerController.Instance.transform.position.y <= maxPosObject.transform.position.y)
                {
                    minDistance = distance;
                    maxPositionObject = maxPosObject;
                }
            }
        }

        CameraMovement.Instance.SetMinMaxPositionObjects(minPositionObject, maxPositionObject);
        SetCurrentRoomByPos();
        ActivatecurrentRoom();
    }

    public void SetCurrentRoomByPos()
    {
        // Setta la stanza corrente in base alla posizione del giocatore
        currentRoom = minPositionObject.transform.parent.gameObject;
    }

    void ActivatecurrentRoom()
    {
        if (currentRoom != null)
            currentRoom.GetComponent<Room>().SpawnObjects();
    }

    public void SetMinMaxPositionObjects(GameObject minPosObject, GameObject maxPosObject)
    {
        minPositionObject = minPosObject;
        maxPositionObject = maxPosObject;
        CameraMovement.Instance.SetMinMaxPositionObjects(minPositionObject, maxPositionObject);
    }
}
