using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxRotation = 45f;

    void Update()
    {
        float angle = maxRotation * Mathf.Sin(Time.time * speed);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}