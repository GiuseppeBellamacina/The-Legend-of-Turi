using UnityEngine;
using TMPro;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public Signals contextOn, contextOff;
    protected GameObject canvas, suggestionBox, dialogBox;
    protected TMP_Text suggestionText, dialogText;
    protected bool playerInRange;
    public string suggestion;
    public bool isContextClue;

    public virtual void Interact(){}
    public virtual void StopInteraction()
    {
        dialogBox.SetActive(false);
        suggestionBox.SetActive(true);
        if (isContextClue)
            contextOn.Raise();
    }

    protected virtual void Awake()
    {
        canvas = GameObject.FindWithTag("Canvas");
        suggestionBox = canvas.transform.Find("Suggestion Box").gameObject;
        suggestionText = suggestionBox.GetComponent<TMP_Text>();
        dialogBox = canvas.transform.Find("Dialog Box").gameObject;
        dialogText = dialogBox.GetComponentInChildren<TMP_Text>();
    }

    protected virtual void Start()
    {
        suggestionBox.SetActive(false);
        dialogBox.SetActive(false);
        contextOff.Raise();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            PlayerController.Instance.SetState(State.none);
            PlayerController.Instance.toInteract = gameObject;
            suggestionText.text = suggestion;
            suggestionBox.SetActive(true);
            if (isContextClue)
                contextOn.Raise();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            PlayerController.Instance.toInteract = null;
            if (suggestionBox != null)
                suggestionBox.SetActive(false);
            contextOff.Raise();
        }
    }
}