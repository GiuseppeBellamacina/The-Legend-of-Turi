using UnityEngine;
using TMPro;

public class Npc : Interactable
{
    protected SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D rb;
    [Header("Dialog")]
    public Dialog dialog;
    public TMP_Text npcTitle;
    public TMP_Text npcDialog;
    public Color titleColor;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        npcTitle = dialogBox.transform.Find("NPC Title").GetComponent<TMP_Text>();
        npcDialog = dialogBox.transform.Find("NPC Dialog").GetComponent<TMP_Text>();
    }

    protected void TextFormatter(string text)
    {
        string title = text.Split(':')[0];
        if (title == "Turi")
            npcTitle.color = new Color(0, 85, 153, 255) / 255f;
        else
            npcTitle.color = titleColor;
        string dialog = text.Split(':')[1];
        npcTitle.text = title;
        npcDialog.text = dialog;
    }

    public override void Interact()
    {
        base.Interact();
        
        suggestionBox.SetActive(false);
        contextOff.Raise();
        dialogBox.SetActive(true);
        dialogText.text = "";
        TextFormatter(dialog.GetFirstSentence());
    }

    public override void ContinueInteraction()
    {
        string sentence = dialog.GetNextSentence();
        if (sentence != null)
        {
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
        
        npcTitle.text = "";
        npcDialog.text = "";
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

    protected virtual void FixedUpdate()
    {
        FixRenderLayer();
    }
}