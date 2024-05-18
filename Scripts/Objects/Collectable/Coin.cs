using UnityEngine;

public class Coin : Collectable
{
    public Signals signal;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.numberOfCoins += item.quantity;
            signal.Raise();
            Destroy(gameObject);
        }
    }
}