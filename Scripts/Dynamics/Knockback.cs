using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool overTimeDamage;
    public float thrust;
    public float attackRate;
    public float knockTime;
    public string otherTag;
    protected float damage;
    private bool isAttacking; // Stato del personaggio che attacca
    private bool isStaggered; // Stato del personaggio che subisce il danno
    private List<GameObject> objectsInContact = new List<GameObject>();

    void SetDamage()
    {
        if (gameObject.CompareTag("PlayerHit"))
            damage = PlayerController.Instance.damage;
        else if (gameObject.CompareTag("Enemy"))
            damage = GetComponent<Character>().data.damage;
        else if (gameObject.CompareTag("EnemyHit"))
            damage = gameObject.GetComponentInParent<Character>().data.damage;
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
        else if (gameObject.CompareTag("EnemyHit"))
        {
            if (gameObject.GetComponentInParent<Character>().IsState(State.attack))
                isAttacking = true;
            else
                isAttacking = false;
        }
    }

    void GetOtherState(GameObject other)
    {
        if (other.CompareTag("Player"))
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
            other.GetComponent<Breakable>().Smash();
            return;
        }

        if (overTimeDamage)
        {
            if (other.gameObject.CompareTag(otherTag) && other.isTrigger && !objectsInContact.Contains(other.gameObject))
            {
                objectsInContact.Add(other.gameObject);
                if (gameObject.activeInHierarchy)
                    StartCoroutine(ApplyDamageOverTime(other.gameObject));
            }
        }
        else
        {
            if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
            {
                HandleKnockback(other.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (overTimeDamage)
        {
            if (objectsInContact.Contains(other.gameObject))
                objectsInContact.Remove(other.gameObject);
        }
    }

    private IEnumerator ApplyDamageOverTime(GameObject other)
    {
        while (objectsInContact.Contains(other))
        {
            if (isAttacking || isStaggered)
                yield return new WaitForSeconds(0.1f);
            HandleKnockback(other);
            yield return new WaitForSeconds(attackRate);
        }
    }

    private void HandleKnockback(GameObject other)
    {
        GetState();
        GetOtherState(other);
        if (!isAttacking || isStaggered)
            return;
        Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
        RigidbodyType2D oldType = hit.bodyType;
        DoDamage(other);
        if (hit != null)
        {
            hit.bodyType = RigidbodyType2D.Dynamic;
            other.GetComponent<Character>().SetState(State.stagger);
            Vector2 difference = hit.transform.position - transform.position;
            difference = difference.normalized * thrust;
            hit.AddForce(difference, ForceMode2D.Impulse);
            other.GetComponent<Character>().Knock(knockTime);
            hit.bodyType = oldType;
        }
    }

    void DoDamage(GameObject other)
    {
        SetDamage();
        other.GetComponent<Character>().TakeDamage(damage);
    }
}
