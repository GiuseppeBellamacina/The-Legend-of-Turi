using UnityEngine;

public enum DoorType
{
    Normal,
    Locked,
    Button,
    Enemies,
    Special
}

public class Door : Interactable
{
    public DoorType doorType;
    public BoolValue isOpen;
    public Inventory inventory;
    public Sprite openSprite;
    SpriteRenderer spriteRenderer;
    Collider2D[] colliders;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
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

    public override void StopInteraction()
    {
        base.StopInteraction();

        dialogBox.SetActive(false);
        if (isOpen.value)
        {
            Open();
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
    }
}