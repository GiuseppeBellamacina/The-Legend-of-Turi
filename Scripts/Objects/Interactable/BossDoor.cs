using UnityEngine;

public class BossDoor : Door
{
    public BoolValue bossDefeated;

    protected override void Start()
    {
        base.Start();

        if (bossDefeated.value)
            Open();
    }

    public override void Interact()
    {
        base.Interact();

        if (doorType == DoorType.Boss)
        {
            if (bossDefeated.value)
            {
                dialogText.text = "La porta si è <b><color=#FF0000FF>aperta</color></b>.";
                // Meccanica di ritorno al castello
                // DA IMPLEMENTARE
                return;
            }
            dialogText.text = "La porta è <b><color=#FF0000FF>bloccata</color></b>.";
        }
    }
}