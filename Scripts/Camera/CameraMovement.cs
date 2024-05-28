using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private static CameraMovement _instance;
    public Transform target;
    public float smoothing; // da inspector
    public GameObject minPositionObject, maxPositionObject; // assegnati in RoomLocator
    public Animator animator;
    public bool isBounded;
    Vector2 minPosition, maxPosition;

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

        animator = GetComponent<Animator>();
        target = PlayerController.Instance.transform;
    }

    void Start()
    {
        SetBoundaries();
        SetInstantPosition();
        isBounded = true;
    }

    IEnumerator FocusCo(Vector3 position, float zoom, float time)
    {
        Instance.enabled = false;
        Instance.animator.enabled = false;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(Camera.main.transform.position, position, elapsedTime / time);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = position;
        Camera.main.orthographicSize = zoom;
    }

    public void Focus(Vector3 position, float zoom, float time){
        StartCoroutine(FocusCo(position, zoom, time));
    }

    public void SetInstantPosition(){
        if (!isBounded)
            transform.position = new (target.position.x, target.position.y, transform.position.z);
        else
        {
            RoomLocator.Instance.FindPositionObjects();
            Vector3 targetPosition = new (target.position.x, target.position.y, transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            transform.position = targetPosition;
        }
    }

    void FixedUpdate()
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
        if (this.minPositionObject == null)
            Debug.LogError("No minPositionObject found.");
        SetBoundaries();
    }

    public void ScreenKick()
    {
        animator.SetBool("kick", true);
        StartCoroutine(ScreenKickCo());
    }

    IEnumerator ScreenKickCo()
    {
        yield return null;
        animator.SetBool("kick", false);
    }
}
