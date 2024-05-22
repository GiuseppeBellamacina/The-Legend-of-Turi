using UnityEngine;

public class Button : Interactable, IResettable
{
    public BoolValue isPressed;
    public Sprite pressedSprite;
    public Signals doorSignal;
    SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        Reset();
    }

    public void Reset()
    {
        if (isPressed.value)
            spriteRenderer.sprite = pressedSprite;
    }

    public override void Interact() {}
    public override void StopInteraction() {}
    public override void ContinueInteraction() {}
    protected override void OnTriggerExit2D(Collider2D other) {}

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        isPressed.value = true;
        spriteRenderer.sprite = pressedSprite;
        doorSignal.Raise();
    }
}