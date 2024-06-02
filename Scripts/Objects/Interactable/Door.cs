using UnityEngine;

public enum DoorType
{
    Normal,
    Locked,
    Button,
    Enemies,
    Special,
    Blocked
}

public class Door : Interactable, IResettable
{
    public DoorType doorType;
    public BoolValue isOpen;
    public Inventory inventory;
    public Sprite openSprite, closedSprite;
    SpriteRenderer spriteRenderer;
    Collider2D[] colliders;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
        Reset();
    }

    public void Reset()
    {
        if (isOpen.value)
            Open();
    }

    public void Open()
    {
        isOpen.value = true;
        if (spriteRenderer != null)
            spriteRenderer.sprite = openSprite;
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = false;
    }

    public void Close()
    {
        isOpen.value = false;
        if (spriteRenderer != null)
            spriteRenderer.sprite = closedSprite;
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;
    }

    public override void StopInteraction()
    {
        base.StopInteraction();

        dialogBox.SetActive(false);
        if (isOpen.value)
        {
            Open();
            PlayerController.Instance.UnlockCharacters();
        }
        else
        {
            suggestionBox.SetActive(true);
            if (isContextClue)
                contextOn.Raise();
        }
    }

    public override void ContinueInteraction()
    {
        StopInteraction();
    }

    public override void Interact()
    {
        base.Interact();
        
        suggestionBox.SetActive(false);
        if (isContextClue)
            contextOff.Raise();
        dialogBox.SetActive(true);
        if (doorType == DoorType.Normal)
        {
            isOpen.value = true;
            dialogText.text = "La porta <b>non era chiusa a chiave</b>, quindi l'hai <b><color=#26663BFF>aperta</color></b>.";
        }
        else if (doorType == DoorType.Locked)
        {
            if (inventory.numberOfKeys > 0)
            {
                isOpen.value = true;
                dialogText.text = "Hai usato una <b><color=#26663BFF>chiave</color></b> per aprire la porta.";
                inventory.UseKey();
            }
            else
                dialogText.text = "La porta è <b><color=#FF0000FF>chiusa</color></b> a chiave, <b>non hai chiavi per aprirla</b>.";
        }
        else if (doorType == DoorType.Button)
        {
            if (!isOpen.value)
            {
                dialogText.text = "La porta è <b><color=#FF0000FF>chiusa</color></b>, forse <b>c'è un modo per aprirla</b>.";
            }
        }
        else if (doorType == DoorType.Blocked)
        {
            dialogText.text = "La porta è <b><color=#FF0000FF>bloccata</color></b>, <b>non c'è modo d'aprirla</b>.";
        }
    }
}