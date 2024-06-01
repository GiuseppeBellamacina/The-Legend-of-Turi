using UnityEngine;

public class BossKnockback : Knockback
{
    public float offset;
    protected override void SetDamage()
    {
        if (gameObject.CompareTag("PlayerHit"))
            damage = PlayerController.Instance.damage;
        else if (gameObject.CompareTag("Enemy"))
            damage = GetComponent<Character>().data.damage;
        else if (gameObject.CompareTag("EnemyHit"))
            damage = gameObject.GetComponentInParent<Character>().data.damage;
        
        damage *= offset;
    }
}