using UnityEngine;

public class Npc : Interactable
{
    protected SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixRenderLayer()
    {
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    protected virtual void Update()
    {
        FixRenderLayer();
    }
}