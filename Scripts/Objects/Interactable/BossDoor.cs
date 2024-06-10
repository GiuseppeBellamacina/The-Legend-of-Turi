using UnityEngine;

public class BossDoor : Door
{
    public BoolValue bossDefeated;
    public GameObject altar;

    protected override void Start()
    {
        base.Start();

        if (bossDefeated.value)
        {
            Open();
            altar.SetActive(true);
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (doorType == DoorType.Boss)
        {
            if (bossDefeated.value)
            {
                dialogText.text = "La porta si è <b><color=#FF0000FF>aperta</color></b> e un <b><color=#CF6B08FF>altare</color></b> si è acceso più sotto.";
                Open();
                altar.SetActive(true);
            }
            else
                dialogText.text = "La porta è <b><color=#FF0000FF>bloccata</color></b>.";
        }
    }
}