using UnityEngine;
using System.Collections;

public class Giusa : Enemy, IResettable
{
    public bool attackReady; // Gestito dagli stati
    public bool isInvulnerable; // Gestito dagli stati
    public float attackCooldown;
    public BoolValue isDead;
    public Dialog dialog;
    public GameObject dialogBox;
    public GameObject soundtrack;
    public AudioClip dungeonMusic;
    int dialogIndex;

    protected override void Start()
    {
        base.Start();
        attackReady = true;
        isInvulnerable = false;
        dialogBox.SetActive(false);
        dialogIndex = 0;

        soundtrack.GetComponent<AudioSource>().Stop();
    }

    public new void Reset()
    {
        if (isDead.value)
        {
            Die();
            return;
        }
        
        health = data.maxHealth;
        transform.position = homePosition;
        SetState(State.idle);
    }

    void FixedUpdate()
    {
        EnemyBehaviour();
    }

    string Talk()
    {
        if (Random.Range(0, Mathf.RoundToInt(health)) == 0)
            return dialog.GetSentence(dialogIndex++ % dialog.sentences.Length);
        else
            return null;
    }

    public void TalkToPlayer()
    {
        string sentence = Talk();
        if (sentence != null)
        {
            dialogBox.SetActive(true);
            dialogBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = sentence;
            StartCoroutine(StopTalking());
        }
        else
            dialogBox.SetActive(false);
    }

    IEnumerator StopTalking()
    {
        yield return new WaitForSeconds(1.5f);
        dialogBox.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        ChooseStagger();
        if (health <= 0)
            Die();
    }

    void ChooseStagger()
    {
        if (health < data.maxHealth * (1.0f / 3.0f))
        {
            animator.SetTrigger("veryStagger");
        }
        else if (health < data.maxHealth * (2.0f / 3.0f))
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    animator.SetTrigger("stagger");
                    break;
                case 1:
                    animator.SetTrigger("veryStagger");
                    break;
            }
        }
        else
            animator.SetTrigger("stagger");
    }

    protected override void Die()
    {
        if (soundtrack.GetComponent<AudioSource>().clip != dungeonMusic)
        {
            soundtrack.GetComponent<AudioSource>().clip = dungeonMusic;
            soundtrack.GetComponent<AudioSource>().Play();
        }
        
        isDead.value = true;
        animator.SetTrigger("die");
        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        isInvulnerable = true;
    }

    protected override IEnumerator KnockCo(float knockTime)
    {
        if (isInvulnerable)
            yield break;

        spriteRenderer.color = Color.red; // Settta lo sprite a rosso
        enabled = false; // Disattiva lo script del personaggio
        yield return new WaitForSeconds(knockTime);
        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = Vector2.zero;
        SetState(State.idle);
        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = Vector2.zero;
        enabled = true; // Riattiva lo script del personaggio
        spriteRenderer.color = Color.white; // Resetta il colore dello sprite
    }

    public override void MoveTo(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        ChangeDirection(direction);
        rb.velocity = data.speed * direction;
    }

    void ChangeDirection(Vector2 direction)
    {
        Quaternion rotation = dialogBox.transform.rotation;
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        dialogBox.transform.rotation = rotation;
    }

    public IEnumerator AttackCooldownCo()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }

    public void AttackCooldown()
    {
        StartCoroutine(AttackCooldownCo());
    }
}