using UnityEngine;

public class ArrowCollect : Collectable
{
    public Signals signal;
    public AudioClip arrowSound;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            AudioManager.Instance.PlaySFX(arrowSound);
            playerInventory.AddArrow(item.quantity);
            signal.Raise();
            Destroy(gameObject);
        }
    }
}