using UnityEngine;
using System.Collections;

public class LogIntro : Log
{
    public float fadeTime = 3f;
    float elapsedTime = 0f;
    bool hasSpawned = false;
    public Vector2 dest;
    public Signals signals;

    protected override void Awake()
    {
        SetState(State.idle);
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        Color tempColor = spriteRenderer.color;
        tempColor.a = 0f;
        spriteRenderer.color = tempColor;
    }
    
    void FadeIn()
    {
        Color startColor = spriteRenderer.color;
        Color targetColor = new (startColor.r, startColor.g, startColor.b, 1f);

        if (elapsedTime < fadeTime)
        {
            Color tempColor = new (startColor.r, startColor.g, startColor.b, elapsedTime / fadeTime);
            spriteRenderer.color = tempColor;
        }
        else
        {
            spriteRenderer.color = targetColor;
            hasSpawned = true;
        }
    }

    void MoveToDest()
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, data.speed * Time.deltaTime);
        ChangeAnim(dest - temp);
        animator.SetBool("wakeUp", true);
        StartCoroutine(StartMoving(temp));
    }

    IEnumerator StartMoving(Vector2 temp)
    {
        while (isWakingUp)
            yield return null;
        rb.MovePosition(temp);
    }

    void Behavior()
    {
        if (!hasSpawned)
        {
            FadeIn();
            elapsedTime += Time.deltaTime;
        }
        else
        {
            MoveToDest();
            if (Vector2.Distance(transform.position, dest) < 0.01f)
            {
                signals.Raise();
                gameObject.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        Behavior();
    }
}