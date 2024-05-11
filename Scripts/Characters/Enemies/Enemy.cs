using UnityEngine;

public class Enemy : Character
{
    protected Transform target;
    public float chaseRange;
    public float attackRange;
    public float health;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        data.health = data.maxHealth;
        health = data.maxHealth;
    }

    protected virtual bool Stop()
    {
        if (PlayerController.Instance.IsState(State.interact))
        {
            SetState(State.none);
            animator.enabled = false;
            rb.velocity = Vector2.zero;
            return true;
        }
        else
        {
            animator.enabled = true;
            return false;
        }
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }
}
