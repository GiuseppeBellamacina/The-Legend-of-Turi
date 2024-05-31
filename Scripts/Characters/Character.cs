using System.Collections;
using UnityEngine;

public enum State
{
    idle,
    walk,
    run,
    chase,
    attack,
    stagger,
    interact,
    none
}

public class Character : MonoBehaviour
{
    [Header("Character References")]
    public State currentState;
    public Rigidbody2D rb;
    public Animator animator;
    public CharacterData data;
    public GameObject deathEffect;
    public SpriteRenderer spriteRenderer;

    [Header("Character Settings")]
    public string characterName;

    protected virtual void Awake()
    {
        SetState(State.idle);
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }

    public bool IsState(State state)
    {
        return currentState == state;
    }

    public void Knock(float knockTime)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(KnockCo(knockTime));
    }

    protected virtual IEnumerator KnockCo(float knockTime)
    {
        spriteRenderer.color = Color.red; // Settta lo sprite a rosso
        enabled = false; // Disattiva lo script del personaggio
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        SetState(State.idle);
        rb.velocity = Vector2.zero;
        enabled = true; // Riattiva lo script del personaggio
        spriteRenderer.color = Color.white; // Resetta il colore dello sprite
    }

    protected virtual void Die()
    {
        if (deathEffect != null){
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    public int GetRenderLayer()
    {
        return spriteRenderer.sortingOrder;
    }

    public virtual void TakeDamage(float damage){}
}
