using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private List<Sprite> durabilitySprites;
    [SerializeField] private List<AudioClip> durabilitySounds;
    [SerializeField] private List<float> durabilityDurations;
    [SerializeField] private float reappearTime = 5f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int currentDurability;
    private bool isCrumbling = false;
    private Coroutine crumbleCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        currentDurability = 0;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isCrumbling)
        {
            crumbleCoroutine = StartCoroutine(Crumble(currentDurability));
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && isCrumbling)
        {
            StopCoroutine(crumbleCoroutine);
            isCrumbling = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isCrumbling)
        {
            crumbleCoroutine = StartCoroutine(Crumble(currentDurability));
        }
    }

    IEnumerator Crumble(int startDurability)
    {
        isCrumbling = true;

        if (startDurability == 0)
        {
            yield return new WaitForSeconds(durabilityDurations[0]);
        }

        for (currentDurability = startDurability; currentDurability < durabilitySprites.Count - 2; currentDurability++)
        {
            spriteRenderer.sprite = durabilitySprites[currentDurability + 1];
            if (currentDurability + 1 < durabilitySounds.Count)
            {
                audioSource.PlayOneShot(durabilitySounds[currentDurability]);
            }

            if (currentDurability + 1 < durabilityDurations.Count)
            {
                yield return new WaitForSeconds(durabilityDurations[currentDurability + 1]);
            }
        }

        // Show the last sprite and play the last sound
        spriteRenderer.sprite = durabilitySprites[durabilitySprites.Count - 1];
        audioSource.PlayOneShot(durabilitySounds[durabilitySounds.Count - 1]);

        // Disable the platform and its colliders
        spriteRenderer.enabled = false;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        StartCoroutine(Reappear());
    }

    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(reappearTime);

        // Re-enable the platform and its colliders
        spriteRenderer.enabled = true;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = true;
        }

        spriteRenderer.sprite = durabilitySprites[0];
        isCrumbling = false;
        currentDurability = 0;
    }

}