using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item;
    public Inventory playerInventory;
    public float lifeTime = 10f;
    float timer;
    SpriteRenderer spriteRenderer;
    public GameObject owner;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(RemoveOwner());
    }

    IEnumerator RemoveOwner()
    {
        if (owner.GetComponent<DeathEffect>())
            yield return new WaitForSeconds(owner.GetComponent<DeathEffect>().duration);
        else
            yield return new WaitForSeconds(0.1f);
        owner = null;
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
        {
            spriteRenderer.sortingOrder = owner.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else if (PlayerController.Instance.transform.position.y < transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 1;
        else if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    protected virtual void FixedUpdate()
    {
        FixRenderLayer();
        timer += Time.deltaTime;
        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}