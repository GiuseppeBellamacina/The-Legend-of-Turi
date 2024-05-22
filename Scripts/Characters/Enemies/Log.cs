using System.Collections;
using UnityEngine;

public class Log : Enemy
{
    protected override void EnemyBehaviour()
    {
        if (Stop())
            return;
            
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
        SetState(State.idle);
        rb.velocity = Vector2.zero;
        animator.SetBool("wakeUp", false);
    }

    void ChaseOrAttack()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, data.speed * Time.deltaTime);
        ChangeAnim(temp - (Vector2)transform.position);
        animator.SetBool("wakeUp", true);
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
        SetState(State.walk);
        rb.bodyType = RigidbodyType2D.Kinematic;
        MoveTo(homePosition);
    }

    void FixedUpdate()
    {
        EnemyBehaviour();
    }
}