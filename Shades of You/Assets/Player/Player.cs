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
    public static activeCharacter ActiveCharacter { get; set; }


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
        if (InputManager.InteractWasPressed)
        {
            Interact();
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
        health -= amount;
        if (health <= 0f)
        {
            Die();
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

    public void Interact()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, mousePosition - (Vector2)Camera.main.transform.position);
    
        if (hit.collider != null && hit.collider.CompareTag("NPC"))
        {
            NPCDialogue npcDialogue = hit.collider.GetComponent<NPCDialogue>();
            if (npcDialogue != null)
            {
                npcDialogue.Interact();
            }
        }
        
        
    }
}