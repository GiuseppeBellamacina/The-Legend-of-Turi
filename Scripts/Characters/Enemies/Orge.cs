using System.Collections;
using UnityEngine;

public class Orge : Enemy, IResettable
{   
    [Header("Orge Settings")]
    public float patrolDuration;
    public bool attackReady; // viene gestito dalle animazioni
    public AudioClip orgeAttack;
    float patrolTimer = 0f;
    Vector2 currentTarget;
    bool patrolEnd = true;
    bool startChase = false;
    bool notSurprised = true;

    public new void Reset()
    {
        health = data.maxHealth;
        transform.position = homePosition;
        patrolEnd = true;
        startChase = false;
        notSurprised = true;
        SetState(State.idle);
    }

    protected override void EnemyBehaviour()
    {
        base.EnemyBehaviour();

        float distance = DistanceToPlayer();
        if ((PlayerInRange(chaseRange) && distance > attackRange - 0.5f) || startChase)
        {
            ChaseOrAttack();
        }
        else if (!startChase)
        {
            if (patrolTimer >= patrolDuration)
            {
                Patrol();
            }
            else
            {
                Idle();
            }
        }
    }

    void ChaseOrAttack()
    {
        if (notSurprised)
        {
            StartCoroutine(Surprise());
            notSurprised = false;
        }
        if (PlayerInRange(attackRange))
            Attack();
        else
            Chase();
    }

    void Idle()
    {
        SetState(State.idle);
        animator.SetBool("isMoving", false);
        rb.velocity = Vector2.zero;
        patrolTimer += Time.deltaTime;
    }

    void Patrol()
    {
        SetState(State.walk);
        animator.SetBool("isMoving", true);

        // Si sposta verso un punto casuale vicino alla sua posizione
        Vector2 patrolTarget = homePosition + Random.insideUnitCircle * chaseRange / 2;
        if (patrolEnd)
        {
            currentTarget = patrolTarget;
            patrolEnd = false;
        }
        MoveTo(currentTarget);

        if (Vector2.Distance(transform.position, currentTarget) < 0.01f)
        {
            patrolEnd = true;
            patrolTimer = 0f;
        }
    }

    void Chase()
    {
        SetState(State.chase);
        startChase = true;
        animator.SetBool("isMoving", true);
        MoveTo(target.position);
    }

    void Attack()
    {
        SetState(State.attack);
        if (!attackReady)
            return;
        
        ChangeAnim(target.position - transform.position);
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        SoundEffect(orgeAttack);
        SetState(State.attack);
        animator.SetBool("attack", true);
        yield return null;
        animator.SetBool("attack", false);
        while (attackReady)
        {
            yield return null;
        }
    }

    void FixedUpdate()
    {
        EnemyBehaviour();
    }
}