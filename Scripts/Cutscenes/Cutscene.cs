using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cutscene : Interactable
{
    public Dialog dialog;
    public BoolValue cutsceneEnded;
    public Color otherTitleColor;
    public bool playOnStart;
    public bool notLock;
    private int dialogIndex;
    private bool speechEnded;

    protected override void Awake()
    {
        if (cutsceneEnded.value)
            Destroy(gameObject);

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        speechEnded = true;

        if (playOnStart)
            PlayerController.Instance.Interact(notLock);
    }

    protected void TextDisplacer(string text)
    {
        if (text == null)
            return;
        
        string title = text.Split(':')[0];
        if (title == "Turi")
            title = "<b><color=#0059FDFF>" + title + "</color></b>";
        else
            title = "<b><color=#" + ColorUtility.ToHtmlStringRGBA(otherTitleColor) + ">" + title + "</color></b>";
        string dialog = text.Split(':')[1];
        dialogText.text = title + ": ";
        dialogBox.SetActive(true);
        if (!speechEnded)
        {
            StopAllCoroutines();
            dialogText.text = title + ": " + dialog;
            speechEnded = true;
            return;
        }
        StartCoroutine(SpeechCo(dialog));
    }

    protected virtual void Speech(string dialog)
    {
        StartCoroutine(SpeechCo(dialog));
    }

    IEnumerator SpeechCo(string dialog)
    {
        speechEnded = false;
        for (int i = 0; i < dialog.Length; i++)
        {
            if (dialog[i] == '<')
            {
                while (dialog[i] != '>')
                {
                    dialogText.text += dialog[i];
                    i++;
                }
                dialogText.text += dialog[i];
                continue;
            }
            dialogText.text += dialog[i];
            yield return new WaitForSeconds(0.03f);
        }
        speechEnded = true;
    }

    public override void Interact()
    {
        base.Interact();

        dialogBox.SetActive(true);
        TextDisplacer(dialog.GetFirstSentence());
        dialogIndex = dialog.dialogIndex;
    }

    public override void ContinueInteraction()
    {
        if (!speechEnded)
        {
            TextDisplacer(dialog.GetSentence(dialogIndex - 1));
            return;
        }

        string sentence = dialog.GetNextSentence();
        if (sentence != null)
        {
            TextDisplacer(sentence);
            dialogIndex = dialog.dialogIndex;
        }
        else
        {
            StopInteraction();
        }
    }

    public override void StopInteraction()
    {
        if (dialogIndex < dialog.sentences.Length - 1 || !speechEnded)
            return;

        PlayerController.Instance.SetState(State.none);
        PlayerController.Instance.toInteract = null;
        playerInRange = false;
        interactionEnded = true;
        dialogBox.SetActive(false);
        cutsceneEnded.value = true;
        PlayerController.Instance.UnlockCharacters();
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerController.Instance.SetState(State.none);
            PlayerController.Instance.toInteract = gameObject;
            playerInRange = true;
            PlayerController.Instance.Interact(notLock);
        }
    }

    protected override void OnTriggerExit2D(Collider2D other){}
}