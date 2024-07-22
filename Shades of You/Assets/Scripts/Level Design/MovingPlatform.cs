using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] public List<Transform> waypoints;
    [SerializeField] public List<float> waypointDurations; // Time duration for each waypoint
    [SerializeField] public float speed = 1.0f;
    public enum MovementType { PingPong, Repeat }
    [SerializeField] public MovementType movementType;

    private int currentWaypointIndex = 0;
    private int direction = 1;
    private Tween movementTween;
    public bool isActive = true;

    void Start()
    {
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        // Only execute if isActive is true
        if (!isActive) return;

        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        movementTween = transform.DOMove(targetWaypoint.position, speed).SetSpeedBased().OnComplete(() =>
        {
            if (movementType == MovementType.PingPong)
            {
                if (currentWaypointIndex == 0)
                {
                    direction = 1; // Move forward
                }
                else if (currentWaypointIndex == waypoints.Count - 1)
                {
                    direction = -1; // Move backward
                }
                currentWaypointIndex += direction; // Move to the next or previous waypoint
            }
            else if (movementType == MovementType.Repeat)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }

            StartCoroutine(WaitAtWaypoint(waypointDurations[currentWaypointIndex]));
        });
    }
    
    IEnumerator WaitAtWaypoint(float duration)
    {
        yield return new WaitForSeconds(duration);
        MoveToNextWaypoint();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().onPlatform = true;
            Debug.Log("Platform detects player entering");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().onPlatform = false;
            Debug.Log("Platform detects player exiting");
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 2)
        {
            return;
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        // If the platform moves in a loop, draw a line from the last waypoint to the first
        if (movementType == MovementType.Repeat)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }
}