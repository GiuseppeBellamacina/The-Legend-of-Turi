using System.Collections;
using UnityEngine;

public class Log : Enemy, IResettable
{
    public SpriteRenderer confuseItem;
    public bool isWakingUp;
    bool notSurprised = true;
    bool isConfused = true;

    public new void Reset()
    {
        health = data.maxHealth;
        transform.position = homePosition;
        notSurprised = true;
        isConfused = true;
        SetState(State.idle);
    }

    protected override void EnemyBehaviour()
    {       
        base.EnemyBehaviour();

        // Se il nemico è in range di inseguimento o attacco
        if (PlayerInRange(chaseRange) && !PlayerInRange(attackRange - 0.5f))
        {
            ChaseOrAttack();
        }
        // Se il nemico è fuori range torna a casa
        else if (!PlayerInRange(chaseRange) && DistanceToHome() > 0.1f)
        {
            GoBack();
        }
        // Se il nemico è tornato a casa si ferma
        else if (DistanceToHome() <= 0.1f)
        {
            Idle();
        }
    }

    void Idle()
    {
        isConfused = true;
        SetState(State.idle);
        rb.velocity = Vector2.zero;
        animator.SetBool("wakeUp", false);
    }

    void ChaseOrAttack()
    {
        isConfused = true;
        if (notSurprised){
            StartCoroutine(Surprise());
            notSurprised = false;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, data.speed * Time.deltaTime);
        ChangeAnim(temp - (Vector2)transform.position);
        animator.SetBool("wakeUp", true);
        StartCoroutine(StartMoving(temp));
    }

    IEnumerator StartMoving(Vector2 temp)
    {
        while (isWakingUp)
            yield return null;
        // Se il nemico è abbastanza vicino attacca
        if (PlayerInRange(attackRange))
            SetState(State.attack);
        // Altrimenti lo insegue
        else
            SetState(State.chase);
        rb.MovePosition(temp);
    }

    void GoBack()
    {
        if (isConfused)
        {
            StartCoroutine(Confuse());
            isConfused = false;
        }
        SetState(State.walk);
        rb.bodyType = RigidbodyType2D.Kinematic;
        notSurprised = true;
        MoveTo(homePosition);
    }

    IEnumerator Confuse()
    {
        confuseItem.enabled = true;
        yield return new WaitForSeconds(0.8f);
        confuseItem.enabled = false;
    }

    void FixedUpdate()
    {
        EnemyBehaviour();
    }
}