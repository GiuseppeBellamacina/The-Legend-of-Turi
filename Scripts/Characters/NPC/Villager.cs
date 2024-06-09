using UnityEngine;

public class Villager : Npc
{
    public bool randomDialog;
    int lastSentence;


    string RandomDialog()
    {
        lastSentence = Random.Range(0, dialog.sentences.Length);
        return dialog.GetSentence(lastSentence);
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
            if (!speechEnded)
            {
                TextDisplacer(dialog.GetSentence(lastSentence));
                return;
            }
            else
                base.StopInteraction();
        }
        else
            base.ContinueInteraction();
    }
}