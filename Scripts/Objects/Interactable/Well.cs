using System.Collections;
using UnityEngine;

public class Well : Interactable
{
    public override void Interact()
    {
        base.Interact();

        suggestionBox.SetActive(false);
        contextOff.Raise();

        if (PlayerController.Instance.inventory.Pay(1))
        {
            dialogText.text = "Hai lanciato una moneta nel pozzo, speriamo che porti fortuna.";
            dialogBox.SetActive(true);
            Luck();
        }
        else
        {
            dialogText.text = "Sei povero, è meglio se neanche ci provi a lanciare una moneta.";
            dialogBox.SetActive(true);
        }
    }

    IEnumerator LuckCo()
    {
        yield return new WaitForSeconds(10f);
        if (Random.Range(0, 100) < 30)
            PlayerController.Instance.Heal(3);
    }

    void Luck()
    {
        StartCoroutine(LuckCo());
    }

    public override void StopInteraction()
    {
        base.StopInteraction();

        dialogBox.SetActive(false);
        suggestionBox.SetActive(true);
        contextOn.Raise();
    }

    public override void ContinueInteraction()
    {
        StopInteraction();
    }
}