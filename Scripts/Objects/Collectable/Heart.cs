using UnityEngine;

public class Heart : Collectable
{
    float healAmount;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            healAmount = item.quantity;
            PlayerController.Instance.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}