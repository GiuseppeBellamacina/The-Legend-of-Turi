using UnityEngine;
using TMPro;
using System.Collections;

public class TreasureChest : Interactable, IResettable
{
    public Item contents;
    public Inventory inventory;
    public bool isOpen;
    public Signals raiseItem;
    Animator anim;

    protected override void Awake()
    {
        base.Awake();
        
        anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        
        Reset();
    }

    public override void Interact()
    {
        base.Interact();
        
        if (!isOpen)
            OpenChest();
        else
            AlreadyOpened();
    }

    public override void ContinueInteraction()
    {
        StopInteraction();
    }

    public override void StopInteraction()
    {
        base.StopInteraction();
        
        dialogBox.SetActive(false);
        if (isOpen)
        {
            if (isContextClue)
                contextOff.Raise();
            isContextClue = false;
            suggestion = "Guarda dentro";
            suggestionText.text = suggestion;
        }
        suggestionBox.SetActive(true);
        if (isContextClue)
            contextOn.Raise();
    }

    IEnumerator OpenChestCo()
    {
        suggestionBox.SetActive(false);
        if (isContextClue)
            contextOff.Raise();
        dialogText.text = contents.description;
        dialogBox.SetActive(true);
        anim.SetBool("opened", true);
        inventory.currentItem = contents;
        raiseItem.Raise();
        contents.hasBeenPickedUp = true;
        while (!PlayerController.Instance.IsState(State.interact)) // non lo toccare
        {
            yield return null;
        }
        isOpen = true;
        inventory.AddItem(contents);
    }

    void OpenChest()
    {
        StartCoroutine(OpenChestCo());
    }

    public void Reset()
    {
        if (contents.hasBeenPickedUp)
        {
            anim.SetBool("opened", true);
            isContextClue = false;
            isOpen = true;
            suggestion = "Guarda dentro";
        }
    }

    void AlreadyOpened()
    {
        isContextClue = false;
        suggestion = "Guarda dentro";
        dialogText.text = "Questo forziere è stato <b><color=#800000FF>già aperto</color></b>.";
        suggestionBox.SetActive(false);
        dialogBox.SetActive(true);
        if (isContextClue)
            contextOff.Raise();
    }
}
