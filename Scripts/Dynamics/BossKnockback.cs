using UnityEngine;

public class BossKnockback : Knockback
{
    protected void SetDamage(float offset)
    {
        if (gameObject.CompareTag("PlayerHit"))
            damage = PlayerController.Instance.damage;
        else if (gameObject.CompareTag("Enemy"))
            damage = GetComponent<Character>().data.damage;
        else if (gameObject.CompareTag("EnemyHit"))
            damage = gameObject.GetComponentInParent<Character>().data.damage;
        
        damage *= offset;
    }

    protected void DoDamage(GameObject other, float offset)
    {
        SetDamage(offset);
        other.GetComponent<Character>().TakeDamage(damage);
    }
}