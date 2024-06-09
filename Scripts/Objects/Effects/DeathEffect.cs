using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public float duration;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void FixRenderLayer()
    {
        if (PlayerController.Instance == null)
            return;
            
        if (PlayerController.Instance.transform.position.y > transform.position.y)
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() + 1;
        else
            spriteRenderer.sortingOrder = PlayerController.Instance.GetRenderLayer() - 2;
    }

    void FixedUpdate()
    {
        FixRenderLayer();
    }
}