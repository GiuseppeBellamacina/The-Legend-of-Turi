using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    [TextArea (3, 10)] public string dialog;

    public override void Interact()
    {
        base.Interact();
        
        suggestionBox.SetActive(false);
        contextOff.Raise();
        dialogText.text = dialog;
        dialogBox.SetActive(true);
    }

    public override void ContinueInteraction()
    {
        StopInteraction();
    }

    public override void StopInteraction()
    {
        base.StopInteraction();
        
        dialogBox.SetActive(false);
        suggestionBox.SetActive(true);
        contextOn.Raise();
    }
}
