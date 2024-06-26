using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public Signals contextOn, contextOff;
    protected GameObject canvas, suggestionBox, dialogBox;
    protected TMP_Text suggestionText, dialogText;
    public string suggestion;
    public bool isContextClue;
    protected bool interactionEnded = false;
    protected bool playerInRange;

    public virtual void Interact()
    {
        AudioManager.Instance.DecreaseMusic(0.5f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.interactionStart);
        interactionEnded = false;
        canvas.transform.Find("Place Text").gameObject.SetActive(false);
    }
    public virtual void StopInteraction()
    {
        AudioManager.Instance.IncreaseMusic(0.5f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.interactionEnd);
        PlayerController.Instance.SetState(State.none);
        interactionEnded = true;
    }
    public virtual void ContinueInteraction(){}

    public bool InteractionEnded()
    {
        return interactionEnded;
    }

    protected virtual void Awake()
    {
        canvas = GameObject.FindWithTag("Canvas");
        canvas = canvas.transform.Find("PlayerUI").gameObject;
        suggestionBox = canvas.transform.Find("Suggestion Box").gameObject;
        suggestionText = suggestionBox.GetComponent<TMP_Text>();
        dialogBox = canvas.transform.Find("Dialog Box").gameObject;
        dialogText = dialogBox.transform.Find("Dialog Text").GetComponent<TMP_Text>();
    }

    protected virtual void Start()
    {
        suggestionBox.SetActive(false);
        dialogBox.SetActive(false);
        if (isContextClue)
            contextOff.Raise();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerController.Instance.SetState(State.none);
            PlayerController.Instance.toInteract = gameObject;
            suggestionText.text = suggestion;
            suggestionBox.SetActive(true);
            playerInRange = true;
            if (isContextClue)
                contextOn.Raise();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.toInteract = null;
            if (suggestionBox != null)
                suggestionBox.SetActive(false);
            playerInRange = false;
            contextOff.Raise();
        }
    }
}