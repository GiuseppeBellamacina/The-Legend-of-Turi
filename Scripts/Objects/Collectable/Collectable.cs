using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item;
    public Inventory playerInventory;
    SpriteRenderer spriteRenderer;
    protected GameObject owner;

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
        if (owner != null)
            spriteRenderer.sortingOrder = owner.GetComponent<SpriteRenderer>().sortingOrder + 1;
        else if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    protected virtual void FixedUpdate()
    {
        FixRenderLayer();
    }
}