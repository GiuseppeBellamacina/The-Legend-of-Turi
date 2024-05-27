using UnityEngine;

public class ArrowCollect : Collectable
{
    public Signals signal;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.AddArrow(item.quantity);
            signal.Raise();
            Destroy(gameObject);
        }
    }
}