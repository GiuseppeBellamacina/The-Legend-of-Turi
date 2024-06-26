using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : Character
{
    private static PlayerController _instance;
    [Header("Player Settings")]
    Vector2 moveDirection;
    float speed;
    public float damage;
    public bool attackReady;
    public bool secondAttackReady;
    [Header("Interactions")]
    public GameObject toInteract;
    GameObject toInteractAux;
    public Signals playerHealthSignal;
    [Header("Extra Data")]
    public FloatValue speedMultiplier;
    public FloatValue damageMultiplier;
    public FloatValue healthMultiplier;
    public Sprite sword, bow;
    public bool firstWeapon;
    [Header("Inventory")]
    public Inventory inventory;
    public GameObject arrow;
    [Header("Renderers")]
    public SpriteRenderer receivedItemSprite;
    public Image weapon;
    public GameObject arrowText;
    [Header("Audio")]
    public AudioClip[] attackClips;
    public AudioClip secondAttackClip;
    public AudioClip treasureClip;
    public AudioClip healClip;
    public AudioClip[] damageClips;
    public AudioSource runngingSource;

    // Actions
    Action<InputAction.CallbackContext> interactAction;
    Action<InputAction.CallbackContext> stopInteractionAction;
    Action<InputAction.CallbackContext> runAction;
    Action<InputAction.CallbackContext> stopRunAction;
    Action<InputAction.CallbackContext> changeWeaponAction;
    Action<InputAction.CallbackContext> attackAction;

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

    public void Start()
    {
        AssignActions();

        // Setto la direzione di default
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        // Inizializzo i parametri
        playerHealthSignal.Raise();
        speed = data.speed;
        damage = data.damage * damageMultiplier.value; // ha valore di default 1 che scende con la difficoltà
        attackReady = true; // viene gestito dalle animazioni
        secondAttackReady = true;
        firstWeapon = true;
        toInteract = null;
        weapon.sprite = sword;
        if (!inventory.hasSword)
            weapon.color = new Color(1, 1, 1, 0);
        else
            CreateAttack();
    }

    public void RemoveActions()
    {
        InputManager.Instance.inputController.Player.Interact.performed -= interactAction;
        InputManager.Instance.inputController.Player.StopInteraction.performed -= stopInteractionAction;
        InputManager.Instance.inputController.Player.Run.performed -= runAction;
        InputManager.Instance.inputController.Player.Run.canceled -= stopRunAction;
        InputManager.Instance.inputController.Player.ChangeWeapon.performed -= changeWeaponAction;
        InputManager.Instance.inputController.Player.Attack.performed -= attackAction;

        InputManager.Instance.inputController.Player.Disable();
    }

    public void AssignActions()
    {
        // Abbino i metodi ai controlli
        interactAction = ctx => Interact();
        stopInteractionAction = ctx => StopInteraction();
        runAction = ctx => Run();
        stopRunAction = ctx => StopRun();
        changeWeaponAction = ctx => ChangeWeapon();

        // Aggiungo i metodi ai controlli
        InputManager.Instance.inputController.Player.Interact.performed += interactAction;
        InputManager.Instance.inputController.Player.StopInteraction.performed += stopInteractionAction;
        InputManager.Instance.inputController.Player.Run.performed += runAction;
        InputManager.Instance.inputController.Player.Run.canceled += stopRunAction;
        InputManager.Instance.inputController.Player.ChangeWeapon.performed += changeWeaponAction;

        if (inventory.hasSword)
            CreateAttack();

        InputManager.Instance.inputController.Player.Enable();
    }

    public void CreateAttack()
    {
        attackAction = ctx => Attack();
        InputManager.Instance.inputController.Player.Attack.performed += attackAction;
        EnableAttack();
    }

    public void DisableAttack()
    {
        if (inventory.hasSword)
        {
            InputManager.Instance.inputController.Player.Attack.Disable();
            InputManager.Instance.inputController.Player.ChangeWeapon.Disable();
            weapon.color = new Color(1, 1, 1, .5f);
        }
    }

    public void EnableAttack()
    {
        if (inventory.hasSword)
        {
            InputManager.Instance.inputController.Player.Attack.Enable();
            InputManager.Instance.inputController.Player.ChangeWeapon.Enable();
            weapon.color = new Color(1, 1, 1, 1);
        }
    }

    void Run()
    {
        if (IsState(State.interact))
        {
            animator.SetBool("isMoving", false);
            return;
        }

        runngingSource.Play();
        SetState(State.run);
        animator.speed = speedMultiplier.value;
        speed = data.speed * speedMultiplier.value;
        DisableAttack();
    }

    void StopRun()
    {
        if (IsState(State.interact))
        {
            animator.SetBool("isMoving", false);
            return;
        }
        
        runngingSource.Stop();
        animator.speed = 1f;
        speed = data.speed;
        EnableAttack();
        if (rb.velocity != Vector2.zero)
            SetState(State.walk);
        else
            SetState(State.idle);
    }

    public void SetArrowText()
    {
        arrowText.GetComponent<TextMeshProUGUI>().text = inventory.numberOfArrows.ToString();
    }

    void ChangeWeapon()
    {
        if (inventory.hasBow)
        {
            if (firstWeapon)
            {
                inventory.SetCurrentItem(inventory.GetItem("Arco"));
                weapon.sprite = bow;
                arrowText.SetActive(true);
                SetArrowText();
                firstWeapon = false;
            }
            else
            {
                inventory.SetCurrentItem(inventory.GetItem("Spada"));
                weapon.sprite = sword;
                arrowText.SetActive(false);
                firstWeapon = true;
            }
        }
        else
            firstWeapon = true;
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
        rb.velocity = moveDirection * speed;
        // Aggiorno l'animazione
        MovementAnimation(lastMoveDirection);
    }

    void MovementAnimation(Vector2 lastMoveDirection)
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", moveDirection.y);
        }
        else
        { // Se non ci si muove, mantengo l'ultima direzione
            animator.SetBool("isMoving", false);
            animator.SetFloat("moveX", lastMoveDirection.x);
            animator.SetFloat("moveY", lastMoveDirection.y);
        }
    }

    void Attack()
    {
        if (firstWeapon)
            MainAttack();
        else
            SecondAttack();
    }

    void MainAttack()
    {   
        if (IsState(State.interact) || !attackReady)
            return;
        
        StartCoroutine(AttackCo());
    }

    bool HasArrow()
    {   
        return inventory.numberOfArrows > 0;
    }

    void SecondAttack()
    {
        if (IsState(State.interact) || !secondAttackReady || !HasArrow())
            return;
        
        inventory.UseArrow();
        StartCoroutine(SecondAttackCo());
    }

    IEnumerator AttackCo()
    {
        RandomSoundEffect(attackClips);
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

    IEnumerator SecondAttackCo()
    {
        SoundEffect(secondAttackClip);
        SetState(State.attack);
        secondAttackReady = false;
        Vector2 attackDirection = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        GameObject projectile = Instantiate(arrow, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetOwner(gameObject);
        projectile.GetComponent<Projectile>().Launch(attackDirection);
        projectile.GetComponent<Arrow>().FixRotation();
        yield return new WaitForSeconds(.5f);
        secondAttackReady = true;
        if (rb.velocity != Vector2.zero)
            SetState(State.walk);
        else
            SetState(State.idle);
    }

    public void LockCharacters(bool notLock = false)
    {
        if (RoomLocator.Instance.currentRoom == null || notLock)
            return;

        GameObject[] obj = RoomLocator.Instance.currentRoom.GetComponent<Room>().objectsToSpawn;
        foreach (GameObject character in obj)
        {
            Character c = character.GetComponent<Character>();
            Npc n = character.GetComponent<Npc>();
            DamagePlayer d = character.GetComponent<DamagePlayer>();

            if (character == toInteract)
                continue;
                
            else if (c != null){
                if (c.rb.bodyType != RigidbodyType2D.Static)
                    c.rb.velocity = Vector2.zero;
                character.GetComponent<Animator>().enabled = false;
                c.enabled = false;
            }
            else if (n != null){
                if (n.rb.bodyType != RigidbodyType2D.Static)
                    n.rb.velocity = Vector2.zero;
                if (character.GetComponent<Animator>() != null)
                    character.GetComponent<Animator>().enabled = false;
                n.enabled = false;
            }
            else if (d != null){
                character.GetComponent<Animator>().enabled = false;
                d.enabled = false;
            }
        }
    }

    public void UnlockCharacters()
    {
        if (RoomLocator.Instance.currentRoom == null)
            return;

        GameObject[] obj = RoomLocator.Instance.currentRoom.GetComponent<Room>().objectsToSpawn;
        foreach (GameObject character in obj)
        {
            Character c = character.GetComponent<Character>();
            Npc n = character.GetComponent<Npc>();
            DamagePlayer d = character.GetComponent<DamagePlayer>();

            if (c != null){
                c.enabled = true;
                character.GetComponent<Animator>().enabled = true;
            }
            else if (n != null){
                n.enabled = true;
                if (character.GetComponent<Animator>() != null)
                    character.GetComponent<Animator>().enabled = true;
            }
            else if (d != null){
                d.enabled = true;
                character.GetComponent<Animator>().enabled = true;
            }
        }
    }

    public void Interact(bool notLock = false)
    {
        if (toInteract == null)
            return;

        if (currentState != State.interact)
        {
            StopRun();
            SetState(State.interact);
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            toInteract.GetComponent<Interactable>().Interact();
            toInteractAux = toInteract;
            rb.velocity = Vector2.zero;
            LockCharacters(notLock);
        }
        else
        {
            toInteract.GetComponent<Interactable>().ContinueInteraction();
            if (toInteract && toInteract.GetComponent<Interactable>().InteractionEnded())
                UnlockCharacters();
        }
    }

    void StopInteraction()
    {
        if (toInteract == null && toInteractAux != null)
            toInteract = toInteractAux;
        else if (toInteract == null)
            return;
        
        UnlockCharacters();
        toInteract.GetComponent<Interactable>().StopInteraction();
        toInteractAux = null;
    }

    public IEnumerator RaiseItemCo()
    {
        AudioManager.Instance.PlaySFX(treasureClip);
        // Il player è già in stato di interazione
        animator.SetBool("isReceiving", true);
        receivedItemSprite.sprite = inventory.currentItem.sprite;
        while (IsState(State.interact))
            yield return null;
        animator.SetBool("isReceiving", false);
        receivedItemSprite.sprite = null;
        inventory.currentItem = null;
        AudioManager.Instance.sfxSource.Stop();
    }

    public void RaiseItem()
    {
        StartCoroutine(RaiseItemCo());
    }

    public void ObtainItem(Item item)
    {
        inventory.AddItem(item);
        inventory.currentItem = item;
        RaiseItem();
    }

    protected override void Die()
    {
        LockCharacters();
        RespawnManager.Instance.Respawn();
    }

    public override void TakeDamage(float damage)
    {
        data.health -= damage * healthMultiplier.value; // ha valore di default 1 che sale con la difficoltà
        CameraMovement.Instance.ScreenKick();
        if (data.health < 0)
            data.health = 0;
        playerHealthSignal.Raise();
        RandomSoundEffect(damageClips);
        if (data.health <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        data.health += amount;
        if (data.health > data.maxHealth)
            data.health = data.maxHealth;
        playerHealthSignal.Raise();
        SoundEffect(healClip);
    }

    public void IncreaseMaxHealth(float amount)
    {
        if (data.maxHealth + amount > data.absoluteMaxHealth)
            data.maxHealth = data.absoluteMaxHealth;
        else
            data.maxHealth += amount;
        data.health = data.maxHealth;
        playerHealthSignal.Raise();
        SoundEffect(healClip);
    }

    void OnMove(InputValue value) // per risolvere il problema dello stato walk
    {
        if (IsState(State.interact))
            return;

        SetState(State.walk);

        if (Cursor.visible)
            Cursor.visible = false;
    }

    void FixedUpdate()
    {
        if (IsState(State.interact))
            return;

        moveDirection = InputManager.Instance.inputController.Player.Move.ReadValue<Vector2>();

        if (moveDirection != Vector2.zero)
            runngingSource.volume = 1;
        else
            runngingSource.volume = 0;
            
        Move(moveDirection);
    }
}