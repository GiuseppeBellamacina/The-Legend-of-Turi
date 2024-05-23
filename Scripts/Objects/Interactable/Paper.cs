using System.Collections;
using UnityEngine;

public class Paper : Interactable
{
    [Header("Paper Settings")]
    public float zoom;
    public float time;
    public GameObject bar, coinHolder;
    float startSize;
    Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        startSize = Camera.main.orthographicSize;
    }

    public override void Interact()
    {
        base.Interact();
        
        suggestionText.text = "Smetti di Leggere";
        contextOff.Raise();

        startPosition = CameraMovement.Instance.transform.position;
        Vector3 position = new (transform.position.x, transform.position.y, CameraMovement.Instance.transform.position.z);
        PlayerController.Instance.spriteRenderer.enabled = false;
        bar.SetActive(false);
        coinHolder.SetActive(false);
        CameraMovement.Instance.Focus(position, zoom, time);
    }

    public override void ContinueInteraction()
    {
        StopInteraction();
    }

    public override void StopInteraction()
    {
        base.StopInteraction();

        CameraMovement.Instance.StopAllCoroutines();
        PlayerController.Instance.spriteRenderer.enabled = true;
        Camera.main.orthographicSize = startSize;
        CameraMovement.Instance.transform.position = startPosition;
        CameraMovement.Instance.animator.enabled = true;
        CameraMovement.Instance.enabled = true;

        suggestionText.text = "Guarda il Volantino";
        suggestionBox.SetActive(true);
        bar.SetActive(true);
        coinHolder.SetActive(true);
        contextOn.Raise();
    }
}