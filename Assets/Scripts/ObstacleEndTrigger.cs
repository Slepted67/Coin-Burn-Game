using UnityEngine;

/// <summary>
/// Attached to EndPoint of an obstacle. Triggers score when player reaches it.
/// </summary>
public class ObstacleEndTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true; // prevent double triggering
            ScoreManager.Instance?.RegisterObstaclePassed();
            Debug.Log("✅ Obstacle passed!");
        }
    }
}