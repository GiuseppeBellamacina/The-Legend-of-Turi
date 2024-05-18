using System.Collections;
using UnityEngine;

public class PlayerController : Character
{
    private static PlayerController _instance;
    [Header("Player Settings")]
    Vector2 moveDirection;
    public bool attackReady;
    [Header("Interactions")]
    public GameObject toInteract;
    public Signals playerHealthSignal;
    [Header("Inventory")]
    public Inventory inventory;
    [Header("Renderers")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer receivedItemSprite;

    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerController>();
                if (_instance == null)
                    Debug.LogError("No PlayerController found in the scene.");
            }
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Abbino i metodi ai controlli
        InputManager.Instance.inputController.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        InputManager.Instance.inputController.Player.Move.canceled += ctx => Move(Vector2.zero);
        InputManager.Instance.inputController.Player.Interact.performed += _ => Interact();
        InputManager.Instance.inputController.Player.StopInteraction.performed += _ => StopInteraction();
        InputManager.Instance.inputController.Player.Attack.performed += _ => Attack(); // FEATURE: metti lo sblocco

        // Setto la direzione di default
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        // Inizializzo i parametri
        data.health = data.maxHealth;
        attackReady = true; // viene gestito dalle animazioni
        toInteract = null;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int GetRenderLayer()
    {
        return spriteRenderer.sortingOrder;
    }

    void Move(Vector2 direction)
    {
        if (IsState(State.interact))
        {
            animator.SetBool("isMoving", false);
            return;
        }
        // Salvo la direzione di movimento
        moveDirection = direction.normalized;
        // Salvo l'ultima direzione di movimento
        Vector2 lastMoveDirection = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        // Applico la velocità al rigidbody
        rb.velocity = moveDirection * data.speed;
        // Aggiorno l'animazione
        MovementAnimation(lastMoveDirection);
    }

    void MovementAnimation(Vector2 lastMoveDirection)
    {
        if (moveDirection != Vector2.zero)
        {
            SetState(State.walk);
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", moveDirection.y);
        }
        else
        { // Se non ci si muove, mantengo l'ultima direzione
            SetState(State.idle);
            animator.SetBool("isMoving", false);
            animator.SetFloat("moveX", lastMoveDirection.x);
            animator.SetFloat("moveY", lastMoveDirection.y);
        }
    }

    void Attack()
    {   
        if (IsState(State.interact) || IsState(State.attack) || !attackReady)
            return;
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        SetState(State.attack);
        animator.SetBool("isAttacking", true);
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(.3f);
        if (rb.velocity != Vector2.zero)
            SetState(State.walk);
        else
            SetState(State.idle);
    }

    void Interact()
    {
        if (toInteract == null)
            return;
        if (currentState != State.interact)
        {
            SetState(State.interact);
            toInteract.GetComponent<Interactable>().Interact();
        }
        else
            toInteract.GetComponent<Interactable>().ContinueInteraction();
    }

    void StopInteraction()
    {
        if (toInteract == null)
            return;
        toInteract.GetComponent<Interactable>().StopInteraction();
    }

    public IEnumerator RaiseItemCo()
    {
        // Il player è già in stato di interazione
        animator.SetBool("isReceiving", true);
        receivedItemSprite.sprite = inventory.currentItem.sprite;
        while (IsState(State.interact))
            yield return null;
        animator.SetBool("isReceiving", false);
        receivedItemSprite.sprite = null;
    }

    public void RaiseItem()
    {
        StartCoroutine(RaiseItemCo());
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        data.health -= damage;
        CameraMovement.Instance.ScreenKick();
        if (data.health < 0)
            data.health = 0;
        playerHealthSignal.Raise();
        if (data.health <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        data.health += amount;
        if (data.health > data.maxHealth)
            data.health = data.maxHealth;
        playerHealthSignal.Raise();
    }
}