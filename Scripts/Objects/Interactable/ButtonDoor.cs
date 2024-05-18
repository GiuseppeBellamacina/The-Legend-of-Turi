using UnityEngine;

public class ButtonDoor : Door
{
    protected override void Start()
    {
        base.Start();

        if (isOpen.value)
            Open();
    }

    public override void Interact()
    {
        base.Interact();

        if (!isOpen.value)
        {
            dialogText.text = "La porta è chiusa, forse c'è un modo per aprirla.";
        }
    }
}