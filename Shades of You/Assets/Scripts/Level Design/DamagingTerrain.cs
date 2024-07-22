using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTerrain : MonoBehaviour
{
    public enum DamageType { InstaKill, DamageOverTime }

    public DamageType damageType; // Set this in the inspector
    public float damagePerTick = 10f; // Set this in the inspector
    public float tickFrequency = 1f; // Set this in the inspector
    public bool isActive = true; // Set this in the inspector

    private Coroutine damageCoroutine;

    public Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || !other.gameObject.CompareTag("Player")) return;

        switch (damageType)
        {
            case DamageType.InstaKill:
                Player.Instance.Die();
                break;
            case DamageType.DamageOverTime:
                damageCoroutine = StartCoroutine(DamageOverTime());
                animator.Play("AnguishFloorTrap_Clip");
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    
    private IEnumerator DamageOverTime()
    {
        while (isActive)
        {
            Player.Instance.DecreaseHealth(damagePerTick);
            yield return new WaitForSeconds(tickFrequency);
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
