using UnityEngine;

public class Villager : Npc
{
    public bool randomDialog;

    string RandomDialog()
    {
        return dialog.sentences[Random.Range(0, dialog.sentences.Length)];
    }

    public override void Interact()
    {
        if (randomDialog)
        {
            interactionEnded = false;
            suggestionBox.SetActive(false);
            contextOff.Raise();
            TextDisplacer(RandomDialog());
        }
        else
            base.Interact();
    }

    public override void ContinueInteraction()
    {
        if (randomDialog)
        {
            base.StopInteraction();
        }
        else
            base.ContinueInteraction();
    }
}