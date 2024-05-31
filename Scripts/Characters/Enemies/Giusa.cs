using UnityEngine;
using System.Collections;

public class Giusa : Enemy
{
    public bool hasPresented; // Se si è presentato, parte il combattimento
    public bool attackReady; // Gestito dagli stati
    public bool isInvulnerable; // Gestito dagli stati
    public float attackCooldown;
    public BoolValue isDead;

    void FixedUpdate()
    {
        EnemyBehaviour();
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        ChooseStagger();
        if (health <= 0)
            Die();
    }

    void ChooseStagger()
    {
        if (health < data.maxHealth * (2.0f / 3.0f))
        {
            animator.SetTrigger("veryStagger");
        }
        else if (health < data.maxHealth / 2)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    animator.SetTrigger("stagger");
                    break;
                case 1:
                    animator.SetTrigger("veryStagger");
                    break;
            }
        }
        else
            animator.SetTrigger("stagger");
    }

    protected override void Die()
    {
        isDead.value = true;
        animator.SetTrigger("die");
        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    protected override IEnumerator KnockCo(float knockTime)
    {
        if (isInvulnerable)
            yield break;

        spriteRenderer.color = Color.red; // Settta lo sprite a rosso
        enabled = false; // Disattiva lo script del personaggio
        yield return new WaitForSeconds(knockTime);
        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = Vector2.zero;
        SetState(State.idle);
        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = Vector2.zero;
        enabled = true; // Riattiva lo script del personaggio
        spriteRenderer.color = Color.white; // Resetta il colore dello sprite
    }

    public override void MoveTo(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        ChangeDirection(direction);
        rb.velocity = data.speed * direction;
    }

    void ChangeDirection(Vector2 direction)
    {
        if (direction.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public IEnumerator AttackCooldownCo()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }

    public void AttackCooldown()
    {
        StartCoroutine(AttackCooldownCo());
    }
}