using UnityEngine;

public class Squirrel : Character
{
    public float range;
    public float timeToMove;
    protected float moveTimer = 0f;
    protected Vector2 homePosition, currentDestination;
    protected bool hasMoved = true;
    protected int nextAction;

    protected override void Awake()
    {
        base.Awake();
        homePosition = transform.position;
    }

    protected virtual void Start()
    {
        nextAction = Random.Range(0, 3);
        SetState(State.walk);
        animator.SetTrigger("walk");
        Idle();
    }

    void Behavior()
    {
        FixRenderLayer();
        Flip();

        if (moveTimer >= timeToMove)
            MoveRandomly();
        else
            Idle();
    }

    void MoveRandomly()
    {
        if (!IsState(State.walk))
        {
            animator.SetTrigger("walk");
            SetState(State.walk);
        }

        Vector2 newPosition = homePosition + Random.insideUnitCircle * range / 2;
        if (hasMoved)
        {
            currentDestination = newPosition;
            hasMoved = false;
        }
        MoveTo(currentDestination);

        if (Vector2.Distance(transform.position, currentDestination) < 0.1f)
        {
            hasMoved = true;
            moveTimer = 0f;
            nextAction = Random.Range(0, 3);
        }
    }

    protected virtual void Idle()
    {
        rb.velocity = Vector2.zero;
        if (!IsState(State.idle))
        {
            switch (nextAction)
            {
                case 0:
                    animator.SetTrigger("idle1");
                    break;
                case 1:
                    animator.SetTrigger("idle2");
                    break;
                case 2:
                    animator.SetTrigger("action");
                    break;
            }
            SetState(State.idle);
        }
        moveTimer += Time.deltaTime;
    }

    protected void MoveTo(Vector2 target)
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, target, data.speed * Time.deltaTime);
        rb.MovePosition(temp);
    }

    protected void FixRenderLayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    protected void Flip()
    {
        Vector2 direction = (Vector2)transform.position - currentDestination;
        if (direction.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    void FixedUpdate()
    {
        Behavior();
    }
}