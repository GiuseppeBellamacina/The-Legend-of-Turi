using System;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public string otherTag;
    private float damage;
    private bool isAttacking;
    private bool isStaggered;

    // Devo prendere il danno dal personaggio che ha attaccato
    // ovvero chi ha effettuato il knockback
    void SetDamage()
    {
        if (gameObject.CompareTag("PlayerHit"))
            damage = PlayerController.Instance.data.damage;
        else if (gameObject.CompareTag("Enemy"))
            damage = GetComponent<Character>().data.damage;
    }

    void GetState()
    {
        if (gameObject.CompareTag("PlayerHit"))
        {
            if (PlayerController.Instance.IsState(State.attack))
                isAttacking = true;
            else
                isAttacking = false;
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            if (GetComponent<Character>().IsState(State.attack))
                isAttacking = true;
            else
                isAttacking = false;
        }
    }

    void GetOtherState(GameObject other)
    {
        if (other.CompareTag("PlayerHit"))
        {
            if (PlayerController.Instance.IsState(State.stagger))
                isStaggered = true;
            else
                isStaggered = false;
        }
        else if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Character>().IsState(State.stagger))
                isStaggered = true;
            else
                isStaggered = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            GetState();
            GetOtherState(other.gameObject);
            if (!isAttacking || isStaggered)
                return;
            Rigidbody2D hit = other.gameObject.GetComponent<Rigidbody2D>();
            RigidbodyType2D oldType = hit.bodyType;
            // Questa parte gestisce il danno
            DoDamage(other.gameObject);
            // Questa parte gestisce il knockback
            if (hit != null)
            {
                hit.bodyType = RigidbodyType2D.Dynamic;
                other.GetComponent<Character>().SetState(State.stagger);
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Character>().Knock(knockTime);
                hit.bodyType = oldType;
            }
        }
        // Questa parte gestisce il danno a oggetti breakable
        else if (other.gameObject.CompareTag("Breakable"))
        {
            other.GetComponent<Breakable>().Smash();
        }
    }

    void DoDamage(GameObject other)
    {
        SetDamage();
        other.GetComponent<Character>().TakeDamage(damage);
    }
}