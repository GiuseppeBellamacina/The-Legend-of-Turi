using System.Collections;
using UnityEngine;

public class Knight : Npc
{
    public bool randomDialog;
    [Header("Patrol Elements")]
    public GameObject iter;
    public bool circularPatrol;
    GameObject[] iterList;
    int iterIndex;
    int offset;
    bool canAttack;
    Vector2 direction;
    public bool patrolTroop, isPatrolling;
    public float speed;
    string lastSentence;

    protected override void Awake()
    {
        base.Awake();
        rb.bodyType = RigidbodyType2D.Static;
        if (patrolTroop){
            isPatrolling = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            iterList = new GameObject[iter.transform.childCount];
            for (int i = 0; i < iter.transform.childCount; i++)
            {
                iterList[i] = iter.transform.GetChild(i).gameObject;
            }
        }
        canAttack = true;
    }

    void Patrol()
    {
        if (iterList.Length == 0)
        {
            return;
        }

        if (Vector2.Distance(transform.position, iterList[iterIndex].transform.position) < 0.1f)
        {
            if (circularPatrol)
            {
                if (iterIndex == iterList.Length - 1)
                    iterIndex = 0;
                else
                    iterIndex++;
            }
            else
            {
                if (iterIndex == iterList.Length - 1)
                {
                    StartCoroutine(WaitAndPatrol());
                    offset = -1;
                }
                else if (iterIndex == 0)
                {
                    StartCoroutine(WaitAndPatrol());
                    offset = 1;
                }
                iterIndex += offset;
            }
        }

        direction = (iterList[iterIndex].transform.position - transform.position).normalized;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.MovePosition((Vector2)transform.position + speed * Time.fixedDeltaTime * direction);
    }

    IEnumerator WaitAndPatrol()
    {
        rb.velocity = Vector2.zero;
        isPatrolling = false;
        rb.bodyType = RigidbodyType2D.Static;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        isPatrolling = true;
    }

    string RandomDialog()
    {
        lastSentence = dialog.GetSentence(Random.Range(0, dialog.sentences.Length));
        return lastSentence;
    }

    public override void Interact()
    {
        if (randomDialog)
        {
            suggestionBox.SetActive(false);
            contextOff.Raise();
            TextDisplacer(RandomDialog());
        }
        else
            base.Interact();
        StopCoroutine(WaitAndPatrol());
    }

    public override void ContinueInteraction()
    {
        if (randomDialog)
        {
            if (!speechEnded)
            {
                TextDisplacer(lastSentence);
                return;
            }
            else
                base.StopInteraction();
        }
        else
        {
            if (!speechEnded)
            {
                TextDisplacer(dialog.GetSentence(dialogIndex - 1));
                return;
            }
            else
                base.ContinueInteraction();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (rb.bodyType == RigidbodyType2D.Kinematic)
                rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            PlayerController.Instance.SetState(State.none);
            PlayerController.Instance.toInteract = gameObject;
            suggestionText.text = suggestion;
            suggestionBox.SetActive(true);
            if (isContextClue)
                contextOn.Raise();
            isPatrolling = false;
            StopCoroutine(WaitAndPatrol());
            playerInRange = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            PlayerController.Instance.toInteract = null;
            if (suggestionBox != null)
                suggestionBox.SetActive(false);
            contextOff.Raise();
            if (patrolTroop)
                isPatrolling = true;
            playerInRange = false;
        }
    }

    void FixDirection()
    {
        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;
    }

    void SimpleKnockback()
    {
        Vector2 difference = PlayerController.Instance.transform.position - transform.position;
        PlayerController.Instance.rb.AddForce(difference.normalized * 6f, ForceMode2D.Impulse);
        PlayerController.Instance.Knock(0.2f);
    }

    void IsBeingAttacked()
    {
        if (PlayerController.Instance.IsState(State.attack) && canAttack)
        {
            StartCoroutine(AttackCo());
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator AttackCo()
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.5f);
        PlayerController.Instance.TakeDamage(1);
        SimpleKnockback();
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (playerInRange)
        {
            LookAtPlayer();
            IsBeingAttacked();
        }
        else if (patrolTroop)
            FixDirection();
        else
            spriteRenderer.flipX = flipped;

        if (patrolTroop && isPatrolling)
        {
            animator.SetBool("isWalking", true);
            Patrol();
        }
        else
            animator.SetBool("isWalking", false);
    }
}