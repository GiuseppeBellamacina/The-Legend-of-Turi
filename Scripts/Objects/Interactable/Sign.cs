using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    [TextArea (3, 10)] public string dialog;

    public override void Interact()
    {
        if (playerInRange && PlayerController.Instance.IsState(State.interact))
        {
            suggestionBox.SetActive(false);
            contextOff.Raise();
            dialogText.text = dialog;
            dialogBox.SetActive(true);
        }
    }
}
