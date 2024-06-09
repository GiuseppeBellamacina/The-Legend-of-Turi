using System.Collections;
using UnityEngine;

public class Enemy : Character, IResettable
{
    public Transform target;
    [Header("Enemy Settings")]
    public float chaseRange;
    public float attackRange;
    public float health;
    [Header("Loot")]
    public GameObject[] loot;
    protected Vector2 homePosition;
    public SpriteRenderer surpriseItem;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindWithTag("Player").transform;
        homePosition = transform.position;
    }

    protected virtual void Start()
    {
        data.health = data.maxHealth;
        health = data.maxHealth;
    }

    public void Reset()
    {
        health = data.maxHealth;
        transform.position = homePosition;
        SetState(State.idle);
    }

    protected bool OverVelocity()
    {
        return rb.velocity.magnitude > data.speed;
    }

    protected virtual void EnemyBehaviour()
    {
        // Sisetma il layer di render
        FixRenderLayer();

        // Se il nemico se ne parte lo limito
        if (OverVelocity())
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, data.speed);
    }

    public bool PlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, target.position) <= range;
    }

    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, target.position);
    }

    protected float DistanceToHome()
    {
        return Vector2.Distance(transform.position, homePosition);
    }

    protected virtual void FixRenderLayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
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
        GameObject drop = loot[random];
        drop.GetComponent<Collectable>().SetOwner(gameObject);
        Instantiate(drop, transform.position, Quaternion.identity);
    }

    protected override void Die()
    {
        base.Die();
        
        DropLoot();
    }

    protected void SetDirection(Vector2 direction)
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

    public virtual void MoveTo(Vector2 target)
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, target, data.speed * Time.deltaTime);
        ChangeAnim(temp - (Vector2)transform.position);
        rb.MovePosition(temp);
    }

    protected IEnumerator Surprise()
    {
        surpriseItem.enabled = true;
        yield return new WaitForSeconds(0.8f);
        surpriseItem.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.isTrigger)
        {
            Vector2 direction = (transform.position - other.transform.position).normalized;
            float force = 5f;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }     
    }
}
