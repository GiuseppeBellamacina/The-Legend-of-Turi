using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private static CameraMovement _instance;
    public Transform target;
    public float smoothing;
    public GameObject minPositionObject, maxPositionObject;
    public bool isBounded;
    Vector2 minPosition, maxPosition;
    bool goToPos;

    public static CameraMovement Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraMovement>();
                if (_instance == null)
                    Debug.LogError("No CameraMovement found in the scene.");
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
        target = PlayerController.Instance.transform;
        SetBoundaries();
        SetInstantPosition();
        isBounded = true;
        goToPos = false;
    }

    IEnumerator GoToPositionCo(Vector3 position, float time){
        goToPos = true;
        while (transform.position != position)
        {
            transform.position = Vector3.Lerp(transform.position, position, smoothing);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        goToPos = false;
    }

    public void GoToPosition(Vector3 position, float time)
    {
        StartCoroutine(GoToPositionCo(position, time));
    }

    public void SetInstantPosition(){
        if (!isBounded)
            transform.position = new (target.position.x, target.position.y, transform.position.z);
        else
        {
            Vector3 targetPosition = new (target.position.x, target.position.y, transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            transform.position = targetPosition;
        }
    }

    void FixedUpdate()
    {
        if (!goToPos)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                if (isBounded)
                {
                    targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                    targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
                }
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
            }
        }
    }

    Vector2 GetCameraDimensions()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Camera.main.aspect;
        return new Vector2(width, height);
    }

    public void SetBoundaries()
    {
        Vector2 cameraDimensions = GetCameraDimensions();
        minPosition = minPositionObject.transform.position;
        maxPosition = maxPositionObject.transform.position;
        minPosition = new Vector2(minPosition.x + cameraDimensions.x / 2, minPosition.y + cameraDimensions.y / 2);
        maxPosition = new Vector2(maxPosition.x - cameraDimensions.x / 2, maxPosition.y - cameraDimensions.y / 2);
    }

    public void SetMinMaxPositionObjects(GameObject minPositionObject, GameObject maxPositionObject)
    {
        this.minPositionObject = minPositionObject;
        this.maxPositionObject = maxPositionObject;
        SetBoundaries();
    }
}
