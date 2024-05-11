using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    none, // lo uso quando il personaggio sta per interagire con un oggetto e all'avvio delle scene
    idle,
    walk,
    attack,
    stagger,
    interact
}

public class Character : MonoBehaviour
{
    public State currentState;
    public Rigidbody2D rb;
    public Animator animator;
    public CharacterData data;
    public string characterName;

    protected virtual void Awake()
    {
        SetState(State.idle);
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }

    public State GetState()
    {
        return currentState;
    }

    public bool IsState(State state)
    {
        return currentState == state;
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo( knockTime));
    }

    IEnumerator KnockCo(float knockTime)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        SetState(State.idle);
        rb.velocity = Vector2.zero;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage){}
}
