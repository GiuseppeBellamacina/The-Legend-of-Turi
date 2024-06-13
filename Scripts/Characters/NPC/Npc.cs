using System.Collections;
using UnityEngine;

public class Npc : Interactable
{
    protected SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D rb;
    protected bool flipped;
    [Header("Dialog")]
    public Dialog dialog;
    public Color titleColor;
    protected int dialogIndex;
    protected bool speechEnded;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        flipped = spriteRenderer.flipX;
        speechEnded = true;
    }

    protected void TextDisplacer(string text)
    {
        if (text == null)
            return;
        
        string title = text.Split(':')[0];
        if (title == "Turi")
            title = "<b><color=#0059FDFF>" + title + "</color></b>";
        else
            title = "<b><color=#" + ColorUtility.ToHtmlStringRGBA(titleColor) + ">" + title + "</color></b>";
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
        
        suggestionBox.SetActive(false);
        contextOff.Raise();
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

        AudioManager.Instance.PlaySFX(AudioManager.Instance.continueInteraction);

        string sentence = dialog.GetNextSentence();
        if (sentence != null)
        {
            suggestionBox.SetActive(false);
            contextOff.Raise();
            dialogBox.SetActive(true);
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
        base.StopInteraction();
        
        speechEnded = true;
        StopAllCoroutines();
        dialogBox.SetActive(false);
        suggestionBox.SetActive(true);
        contextOn.Raise();
    }

    protected virtual void FixRenderLayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    protected void LookAtPlayer()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    protected virtual void FixedUpdate()
    {
        FixRenderLayer();

        if (playerInRange)
            LookAtPlayer();
        else
            spriteRenderer.flipX = flipped;
    }
}