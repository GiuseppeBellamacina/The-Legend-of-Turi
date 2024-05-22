using System.Collections;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    [Header("Loot")]
    public GameObject[] loot;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Smash()
    {
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
        Instantiate(loot[random], transform.position, Quaternion.identity);
    }

    void FixRenderLayer()
    {
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
