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

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        flipped = spriteRenderer.flipX;
    }

    protected void TextFormatter(string text)
    {
        if (text == null)
            return;
        
        string title = text.Split(':')[0];
        if (title == "Turi")
            title = "<b><color=#0059FDFF>" + title + "</color></b>";
        else
            title = "<b><color=#" + ColorUtility.ToHtmlStringRGBA(titleColor) + ">" + title + "</color></b>";
        string dialog = text.Split(':')[1];
        dialogText.text = title + ": " + dialog;
    }

    public override void Interact()
    {
        base.Interact();
        
        suggestionBox.SetActive(false);
        contextOff.Raise();
        dialogBox.SetActive(true);
        TextFormatter(dialog.GetFirstSentence());
    }

    public override void ContinueInteraction()
    {
        string sentence = dialog.GetNextSentence();
        if (sentence != null)
        {
            suggestionBox.SetActive(false);
            contextOff.Raise();
            dialogBox.SetActive(true);
            TextFormatter(sentence);
        }
        else
        {
            StopInteraction();
        }
    }

    public override void StopInteraction()
    {
        base.StopInteraction();
        
        dialogBox.SetActive(false);
        suggestionBox.SetActive(true);
        contextOn.Raise();
    }

    protected virtual void FixRenderLayer()
    {
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