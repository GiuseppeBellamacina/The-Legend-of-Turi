using UnityEngine;

public class Log : Enemy
{
    [Header("Log Settings")]
    public Vector3 homePosition;

    protected override void Start()
    {
        base.Start();
        homePosition = transform.position;
    }

    public virtual void CheckDistance()
    {
        // Se il player sta interagendo con qualcosa, il nemico non si muove
        if (Stop())
            return;
            
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        // Se il nemico è vicino: segui o attacca
        if (distanceToTarget <= chaseRange && distanceToTarget >= attackRange - 0.5f)
        {
            if (currentState != State.stagger)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, data.speed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                animator.SetBool("wakeUp", true);
                if (distanceToTarget <= attackRange)
                    SetState(State.attack);
                else
                    SetState(State.walk);
                rb.MovePosition(temp);
            }
        }
        // Se il nemico è lontano: torna indietro
        else if (distanceToTarget > chaseRange && Vector3.Distance(homePosition, transform.position) != 0)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            SetState(State.walk);
            Vector3 temp = Vector3.MoveTowards(transform.position, homePosition, data.speed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            rb.MovePosition(temp);
        }
        // Appena arriva a casa: idle
        else if (Vector3.Distance(homePosition, transform.position) == 0)
        {
            animator.SetBool("wakeUp", false);
            SetState(State.idle);
        }
    }

    void SetDirection(Vector2 direction)
    {
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    protected void ChangeAnim(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                SetDirection(Vector2.right);
            else if (direction.x < 0)
                SetDirection(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
                SetDirection(Vector2.up);
            else if (direction.y < 0)
                SetDirection(Vector2.down);
        }
    }

    protected override void FixedUpdate()
    {
        CheckDistance();
        FixRenderLayer();
    }
}