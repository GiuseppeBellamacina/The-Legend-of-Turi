using UnityEngine;

public class PortaCuore : Collectable
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerController.Instance.IncreaseMaxHealth(item.quantity);
            Destroy(gameObject);
        }
    }
}