using UnityEngine;
using System.Collections;

public class Arrow : Projectile
{
    float newDamage;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        newDamage = PlayerController.Instance.damage * 0.75f;
    }

    public void FixRotation()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se collide con il proprietario, non fare nulla
        if (collision.gameObject == owner)
            return;

        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy>().health <= 0)
                return;
                
            collision.GetComponent<Enemy>().TakeDamage(newDamage);
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
        Destroy(gameObject);
    }

    IEnumerator Hitted(Collider2D collision)
    {
        collision.GetComponent<Character>().Knock(0.1f);
        yield return new WaitForSeconds(0.1f);
    }
}