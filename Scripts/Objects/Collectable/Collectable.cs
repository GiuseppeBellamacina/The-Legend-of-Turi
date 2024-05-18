using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item;
    public Inventory playerInventory;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}