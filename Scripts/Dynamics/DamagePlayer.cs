using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float damage;
    public float timeBetweenDamage;
    float timeSinceLastDamage = 0;
    bool canDamage = false;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        //if (PlayerController.Instance == null)
            //return;

        FixRenderLayer();
        timeSinceLastDamage += Time.deltaTime;
        if (canDamage)
        {
            if (timeSinceLastDamage >= timeBetweenDamage)
            {
                PlayerController.Instance.TakeDamage(damage);
                StartCoroutine(SetSpriteColorCo());
                timeSinceLastDamage = 0;
            }
        }
    }

    void FixRenderLayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    IEnumerator SetSpriteColorCo()
    {
        PlayerController.Instance.spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.spriteRenderer.color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && timeSinceLastDamage >= timeBetweenDamage)
        {
            canDamage = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canDamage = false;
        }
    }
}
