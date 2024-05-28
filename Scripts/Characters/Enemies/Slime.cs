using System.Collections;
using UnityEngine;

public class Slime : Enemy, IResettable
{
    [Header("Projectile Settings")]
    public GameObject projectile;
    public float coolDown;
    [Header("Movement Settings")]
    public float moveTimer;
    public float maxDistance;
    float moveTimerCounter, coolDownCounter;
    bool hasAttacked;

    protected override void Start()
    {
        base.Start();
        chaseRange = attackRange;
    }

    public new void Reset()
    {
        health = data.maxHealth;
        transform.position = homePosition;
        hasAttacked = false;
        SetState(State.idle);
    }

    void FixedUpdate()
    {
        EnemyBehaviour();
    }

    protected override void EnemyBehaviour()
    {
        base.EnemyBehaviour();

        moveTimerCounter += Time.deltaTime;
        coolDownCounter += Time.deltaTime;

        if (moveTimerCounter >= moveTimer)
        {
            if (FarFromHome())
                MoveToPosition(homePosition);
            else
                MoveRandom();
            moveTimerCounter = 0;
        }
        if (CanAttack())
            Attack();
        else
            SetState(State.idle);
    }

    void Attack()
    {
        if (IsState(State.attack))
            return;

        StartCoroutine(AttackCo());
    }

    void MoveToPosition(Vector2 position)
    {
        if (IsState(State.attack))
            return;

        SetState(State.walk);
        if (Random.Range(0, 2) == 0)
            animator.SetTrigger("jump");
        Vector2 direction = (position - (Vector2)transform.position).normalized;
        rb.velocity = direction * data.speed;
        StartCoroutine(StopMoving());
    }

    bool FarFromHome()
    {
        return Vector2.Distance(transform.position, homePosition) > maxDistance;
    }

    void MoveRandom()
    {
        if (IsState(State.attack))
            return;

        SetState(State.walk);
        if (Random.Range(0, 5) == 0)
            animator.SetTrigger("jump");
        float randomX = Random.Range(-maxDistance, maxDistance);
        float randomY = Random.Range(-maxDistance, maxDistance);
        Vector2 direction = new Vector2(randomX, randomY);
        rb.velocity = direction.normalized * data.speed;
        StartCoroutine(StopMoving());
    }

    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
        SetState(State.idle);
    }

    IEnumerator AttackCo()
    {
        rb.velocity = Vector2.zero;
        SetState(State.attack);
        ChangeDirectionToAttack();
        animator.SetTrigger("attack");
        rb.velocity = Vector2.zero; // Fermo il nemico
        yield return new WaitForSeconds(.1f);
        coolDownCounter = 0;
        hasAttacked = true;
        Vector2 direction = target.transform.position - transform.position;
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.GetComponent<Projectile>().SetOwner(gameObject);
        proj.GetComponent<Projectile>().Launch(direction);
        // Attendi che il proiettile colpisca il bersaglio
        StartCoroutine(StopAttacking(proj));
    }

    IEnumerator StopAttacking(GameObject proj)
    {
        rb.velocity = Vector2.zero;
        SetState(State.idle);
        while (proj != null)
        {
            yield return null;
        }
        hasAttacked = false;
    }

    bool CanAttack()
    {
        bool inRange = Vector2.Distance(transform.position, target.transform.position) < attackRange;
        bool canAttack = coolDownCounter >= coolDown;
        return inRange && canAttack && !hasAttacked;
    }

    void ChangeDirectionToAttack()
    {
        Vector2 direction = target.transform.position - transform.position;
        if (direction.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}