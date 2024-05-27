using System.Collections;
using UnityEngine;

public class RockProjectile : Projectile
{
    bool deflected;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se collide con il proprietario, non fare nulla
        if (collision.gameObject == owner)
            return;

        else if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage);
            StartCoroutine(Hitted(collision));
        }
        else if (collision.CompareTag("Breakable"))
        {
            collision.GetComponent<Breakable>().Smash();
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        else if (deflected && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            StartCoroutine(Hitted(collision));
        }
        else if (collision.CompareTag("PlayerHit"))
        {
            Deflect();
            return;
        }
        Destroy(gameObject);
    }

    IEnumerator Hitted(Collider2D collision)
    {
        collision.GetComponent<Character>().Knock(0.1f);
        yield return new WaitForSeconds(0.1f);
    }

    void Deflect()
    {
        // Cambia il proprietario del proiettile ed il layer
        SetOwner(PlayerController.Instance.gameObject);
        gameObject.layer = LayerMask.NameToLayer("Arrow");
        // Inverti la direzione del proiettile e resetta il tempo di vita
        Vector2 direction = PlayerController.Instance.transform.position - transform.position;
        lifeTimeCounter = 0;
        Launch(-direction);
        // Attivo la collisione con il nemico
        deflected = true;
    }
}