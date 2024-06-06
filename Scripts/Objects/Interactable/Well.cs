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
            dialogText.text = "Hai lanciato una <b><color=#CF6B08FF>moneta</color></b> nel pozzo, speriamo che porti <b><color=#26663BFF>fortuna</color></b>.";
            dialogBox.SetActive(true);
            Luck();
        }
        else
        {
            dialogText.text = "Sei <b><color=#FF0000FF>povero</color></b>, è meglio se neanche ci provi a lanciare una <b><color=#CF6B08FF>moneta</color></b>.";
            dialogBox.SetActive(true);
        }
    }

    IEnumerator LuckCo()
    {
        yield return new WaitForSeconds(3f);
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