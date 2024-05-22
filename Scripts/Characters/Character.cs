using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    idle,
    walk,
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
            StartCoroutine(KnockCo( knockTime));
    }

    IEnumerator KnockCo(float knockTime)
    {
        spriteRenderer.color = Color.red; // Settta lo sprite a rosso
        GetComponent<Character>().enabled = false; // Disattiva lo script del personaggio
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        SetState(State.idle);
        rb.velocity = Vector2.zero;
        GetComponent<Character>().enabled = true; // Riattiva lo script del personaggio
        spriteRenderer.color = Color.white; // Resetta il colore dello sprite
    }

    protected virtual void Die()
    {
        if (deathEffect != null){
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage){}
}
