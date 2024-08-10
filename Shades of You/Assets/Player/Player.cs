using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Vector2 checkpoint; // Made public to be accessible from other scripts
    private Rigidbody2D _rb; // Made private as it's only used in this script

    public static Player Instance { get; private set; }

    public enum activeCharacter { Blake, Iara, SooYeon, Xesus }
    [SerializeField] public activeCharacter ActiveCharacter;

    [Header("Skill Variables")]
    [SerializeField] public bool isInvulnerable = false;
    [SerializeField] public bool isDashing = false;
    [SerializeField] public float dashForce = 20f;
    [SerializeField] public bool isInvisible = false;
    [SerializeField] public float invisibleTime = 0.8f;
    [SerializeField] public float invisibleAlpha = 0.2f;
    [SerializeField] public bool isSlowFalling = false;
    [SerializeField] public float fallSpeedReduction = 5f;

    public float abilityCooldown = 0f;
    public float abilityMaxCooldown = 5f;
    private float abilityTimer = 0f;
    private bool isUsingAbility = false;

    private bool isNearNPC = false;
    private NPCDialogue npcDialogue;

    [Header("Respawn Time")]
    [SerializeField] private float respawnDelay = 2f;
    [Header("Health")]
    [SerializeField] public float health = 100f;
    private SpriteRenderer _sprite;

    void Start()
    {
        checkpoint = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.InteractWasPressed && isNearNPC)
        {
            npcDialogue.Interact();
        } 

        // Check if the ability key was pressed and the ability is not on cooldown
        if (InputManager.AbilityWasPressed && abilityCooldown <= 0f && !isUsingAbility)
        {
            UseSkill();
            abilityCooldown = abilityMaxCooldown;
            isUsingAbility = true;
            abilityTimer = 0f;
        }

        // Check if the ability key is still being pressed for all abilities except Dash
        if (InputManager.AbilityIsHeld && ActiveCharacter != activeCharacter.SooYeon)
        {
            abilityTimer += Time.deltaTime;
            if (abilityTimer > 2f)
            {
                StopSkill();
                abilityCooldown = abilityMaxCooldown;
                isUsingAbility = false;
                abilityTimer = 0f;
            }
        }
        else if (!InputManager.AbilityIsHeld)
        {
            isUsingAbility = false;
        }

        // Decrease the cooldown over time, but don't let it go below 0
        if (abilityCooldown > 0f && !isUsingAbility)
        {
            abilityCooldown = Mathf.Max(0f, abilityCooldown - Time.deltaTime);
        }    
    }

    // Method to update the checkpoint
    public void UpdateCheckpoint(Vector2 newCheckpoint)
    {
        checkpoint = newCheckpoint;
    }

    public void Die()
    {
        // Tween the player's alpha to 0
        _sprite.DOFade(0, 1f).OnComplete(() =>
        {
            StartCoroutine(Respawn());
        });
    }

    public void DecreaseHealth(float amount)
    {
        if (!isInvulnerable)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    // Method to replenish health
    public void ReplenishHealth(float amount)
    {
        health += amount;
        if (health > 100f)
        {
            health = 100f;
        }
    }

    // Coroutine to respawn the player after a delay
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        transform.position = checkpoint;
        _sprite.DOFade(1, 1f); 
        health = 100f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            isNearNPC = true;
            npcDialogue = collision.GetComponent<NPCDialogue>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            isNearNPC = false;
            npcDialogue = null;

            DialogueManager.Instance.EndDialogue();
        }
    }

    public void SetActiveCharacter(CharacterData characterData)
    {
        ActiveCharacter = characterData.characterName;
        _sprite.sprite = characterData.characterSprite;
        GetComponent<Animator>().runtimeAnimatorController = characterData.characterAnimation;
        // Implement the unique skill functionality
    }

    public void UseSkill()
    {
        switch (ActiveCharacter)
        {
            case activeCharacter.Blake:
                BecomeInvulnerable();
                break;
            case activeCharacter.SooYeon:
                DashForward();
                break;
            case activeCharacter.Xesus:
                BecomeInvisible();
                break;
            case activeCharacter.Iara:
                ReduceMaxFallSpeed();
                break;
        }
    }

    private void BecomeInvulnerable()
    {
        isInvulnerable = true;
    }

    private void DashForward()
    {
        float direction = transform.localScale.x;

        GetComponent<Rigidbody2D>().AddForce(new Vector2(dashForce * direction, 0f), ForceMode2D.Impulse);
    }

    private void BecomeInvisible()
    {
        isInvisible = true;
        StartCoroutine(ChangeTransparency(invisibleAlpha, invisibleTime));
    }

    private void BecomeVisible()
    {
        isInvisible = false;
        StartCoroutine(ChangeTransparency(1f, invisibleTime));
    }

    private IEnumerator ChangeTransparency(float targetAlpha, float duration)
    {
        // Get the player's sprite renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the original color
        Color originalColor = spriteRenderer.color;

        // Calculate the rate of change
        float rate = 1f / duration;

        // Interpolate the alpha value
        for (float t = 0f; t <= 1f; t += Time.deltaTime * rate)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalColor.a, targetAlpha, t));
            yield return null;
        }
    }

    private void ReduceMaxFallSpeed()
    {
        GetComponent<PlayerMovement>().fallSpeedModifier = fallSpeedReduction;
    }

    private void ResetMaxFallSpeed()
    {
        GetComponent<PlayerMovement>().fallSpeedModifier = 0f;
    }


    private void StopSkill()
    {
        isUsingAbility = false;

        if (isInvulnerable = true)
        {
            isInvulnerable = false;
        }
        else if (isDashing = true)
        {
            isDashing = false;
        }
        else if (isInvisible = true)
        {
            isInvisible = false;
            BecomeVisible();
        }
        else if (isSlowFalling = true)
        {
            isSlowFalling = false;
            ResetMaxFallSpeed();
        }
    }

}