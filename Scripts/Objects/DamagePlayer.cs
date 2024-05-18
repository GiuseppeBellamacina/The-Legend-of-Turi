using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float damage;
    public float timeBetweenDamage;
    private float timeSinceLastDamage = 0;

    void Update()
    {
        timeSinceLastDamage += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && timeSinceLastDamage >= timeBetweenDamage)
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
            timeSinceLastDamage = 0;
        }
    }
}
