using System.Collections;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public AudioClip smashSound;
    [Header("Loot")]
    public GameObject[] loot;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Smash()
    {
        AudioManager.Instance.PlaySFX(smashSound);
        animator.SetBool("isSmashed", true);
        DropLoot();
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
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

    void FixRenderLayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    void FixedUpdate()
    {
        FixRenderLayer();
    }
}
