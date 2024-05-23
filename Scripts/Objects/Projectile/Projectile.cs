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
    Vector2 currentVelocity;
    bool isPaused;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTimeCounter = 0;
    }

    protected virtual void Update()
    {
        // Se si era in pausa e il proprietario è attivo, riprendi il movimento
        if (isPaused && owner.activeInHierarchy)
        {
            rb.velocity = currentVelocity;
            isPaused = false;
        }

        // Se non è in pausa e il proprietario è attivo, incrementa il tempo di vita
        if(!isPaused && owner.activeInHierarchy)
            lifeTimeCounter += Time.deltaTime;

        // Se il tempo di vita è maggiore o uguale al tempo di vita massimo, distruggi l'oggetto
        if (lifeTimeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
        // Se il proprietario è disattivato, metti in pausa il movimento
        else if (owner.GetComponent<Character>().enabled == false)
        {
            currentVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
            isPaused = true;
        }
        // Se il proprietario è disattivato e il gioco non è in pausa, distruggi l'oggetto
        else if (!owner.activeInHierarchy && !isPaused)
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