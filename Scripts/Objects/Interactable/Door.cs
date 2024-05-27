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
        spriteRenderer.sprite = openSprite;
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = false;
        isContextClue = false;
    }

    public void Close()
    {
        isOpen.value = false;
        spriteRenderer.sprite = closedSprite;
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;
        isContextClue = true;
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
            dialogText.text = "La porta non era chiusa a chiave, quindi l'hai aperta.";
        }
        else if (doorType == DoorType.Locked)
        {
            if (inventory.numberOfKeys > 0)
            {
                isOpen.value = true;
                dialogText.text = "Hai usato una chiave per aprire la porta.";
                inventory.UseKey();
            }
            else
                dialogText.text = "La porta è chiusa a chiave, non hai chiavi per aprirla.";
        }
        else if (doorType == DoorType.Button)
        {
            if (!isOpen.value)
            {
                dialogText.text = "La porta è chiusa, forse c'è un modo per aprirla.";
            }
        }
        else if (doorType == DoorType.Blocked)
        {
            dialogText.text = "La porta è bloccata, non c'è modo d'aprirla.";
        }
    }
}