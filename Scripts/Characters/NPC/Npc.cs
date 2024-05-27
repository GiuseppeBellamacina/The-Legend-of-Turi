using UnityEngine;

public class Npc : Interactable
{
    protected SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D rb;
    [Header("Dialog")]
    public Dialog dialog;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Interact()
    {
        base.Interact();
        
        suggestionBox.SetActive(false);
        contextOff.Raise();
        dialogBox.SetActive(true);
        dialogText.text = dialog.GetFirstSentence();
    }

    public override void ContinueInteraction()
    {
        string sentence = dialog.GetNextSentence();
        if (sentence != null)
        {
            dialogText.text = sentence;
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

    protected virtual void Update()
    {
        FixRenderLayer();
    }
}