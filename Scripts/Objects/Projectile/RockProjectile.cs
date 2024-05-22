using UnityEngine;

public class RockProjectile : Projectile
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // if it hitted the enemy who shot it, ignore it
        if (collision.gameObject == owner)
            return;
        else if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage);
        }
        else if (collision.CompareTag("Breakable"))
        {
            collision.GetComponent<Breakable>().Smash();
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}