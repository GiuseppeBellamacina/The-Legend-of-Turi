using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item;
    public Inventory playerInventory;
    SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.AddItem(item);
            Destroy(gameObject);
        }
    }

    protected virtual void FixRenderLayer()
    {
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    protected virtual void FixedUpdate()
    {
        FixRenderLayer();
    }
}