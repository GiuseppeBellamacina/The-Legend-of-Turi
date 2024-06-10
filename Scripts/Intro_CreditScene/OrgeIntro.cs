using UnityEngine;

public class OrgeIntro : Orge
{
    public Vector2 dest;
    public Signals signals;

    protected override void Awake()
    {
        SetState(State.idle);
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isMoving", true);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void MoveToDest()
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, data.speed * Time.deltaTime);
        ChangeAnim(dest - temp);
        animator.SetBool("isMoving", true);
        rb.MovePosition(temp);
    }

    void Behavior()
    {
        MoveToDest();
        if (Vector2.Distance(transform.position, dest) < 0.01f)
        {
            signals.Raise();
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Behavior();
    }
}