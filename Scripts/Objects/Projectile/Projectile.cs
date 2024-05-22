using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage;
    [Header("Movement Settings")]
    public float speed;
    [Header("LifeTime Settings")]
    public float lifeTime;
    protected float lifeTimeCounter;
    protected Rigidbody2D rb;
    protected GameObject owner;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTimeCounter = 0;
    }

    protected virtual void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public virtual void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public virtual void Launch(Vector2 initialVel)
    {
        rb.velocity = initialVel.normalized * speed;
    }
}