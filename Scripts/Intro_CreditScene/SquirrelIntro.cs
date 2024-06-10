using System.Collections;
using UnityEngine;

public class SquirrelIntro : Squirrel
{
    public GameObject[] destinationsObj;
    public bool callAtEnd;
    public float time;
    Vector2[] destinations;
    int nextDest = 0;
    public Signals signals;
    bool coroutine;

    protected override void Awake()
    {
        base.Awake();
        destinations = new Vector2[destinationsObj.Length];
        for (int i = 0; i < destinationsObj.Length; i++)
            destinations[i] = destinationsObj[i].transform.position;
    }

    protected override void Start()
    {
        base.Start();
        animator.SetTrigger("walk");

        if (!callAtEnd)
            StartCoroutine(Timer(time));
    }

    void Behavior()
    {
        Flip();

        if (moveTimer >= timeToMove)
            MoveToNextDest();
        else
            Idle();
    }

    protected override void Idle()
    {
        rb.velocity = Vector2.zero;
        if (IsState(State.walk))
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

    void MoveToNextDest()
    {
        if (IsState(State.idle))
        {
            animator.SetTrigger("walk");
            SetState(State.walk);
        }

        if (hasMoved)
        {
            nextDest++;
            if (nextDest >= destinations.Length)
            {
                if (callAtEnd)
                    signals.Raise();
                gameObject.SetActive(false);
            }
            currentDestination = destinations[nextDest % destinations.Length];
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

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        signals.Raise();
    }

    void FixedUpdate()
    {
        Behavior();
    }
}