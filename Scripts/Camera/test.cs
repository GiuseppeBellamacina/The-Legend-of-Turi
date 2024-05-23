using UnityEngine;

public class test : MonoBehaviour
{
    float oldSize;

    void Start()
    {
        oldSize = Camera.main.orthographicSize;
        CameraMovement.Instance.Focus(new Vector3(0, 0, -10), 2, 5);
    }
}