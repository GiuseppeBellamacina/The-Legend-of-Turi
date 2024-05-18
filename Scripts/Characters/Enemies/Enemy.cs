using UnityEngine;

public class Enemy : Character
{
    protected Transform target;
    [Header("Enemy Settings")]
    public float chaseRange;
    public float attackRange;
    public float health;
    [Header("Loot")]
    public GameObject[] loot;
    protected SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        data.health = data.maxHealth;
        health = data.maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixRenderLayer()
    {
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    protected virtual bool Stop()
    { // Questa funzione si deve mettere nella funzione di movimento del nemico
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

    protected virtual void DropLoot()
    {
        if (loot.Length == 0)
            return;
        int random = Random.Range(0, loot.Length);
        Instantiate(loot[random], transform.position, Quaternion.identity);
    }

    protected override void Die()
    {
        base.Die();
        
        DropLoot();
    }

    protected virtual void FixedUpdate()
    {
        FixRenderLayer();
    }
}
