using UnityEngine;

public class SpecialDoor : Door
{
    public BoolValue willOpen;

    public override void Interact()
    {
        base.Interact();

        if (doorType == DoorType.Special)
        {
            if (!willOpen.value)
            {
                dialogText.text = "La porta <b><color=#FF0000FF>non si muove</color></b>, una scritta in rilievo dice:\n<i>\"Se con <b><color=#FF0000FF>Il Senza Confronti</color></b> ti vuoi confrontare, con la sua amata devi parlare\"</i>";
            }
            else
            {
                dialogText.text = "La porta si è <b><color=#26663BFF>aperta magicamente</color></b>.\nMentre si apriva hai sentivo una voce dire:\n<i>\"<b><color=#FF0000FF>Vieni a cercarmi</color></b>\"</i>";
                isOpen.value = true;
            }
        }
    }
}