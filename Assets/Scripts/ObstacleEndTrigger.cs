using UnityEngine;

/// <summary>
/// Attached to the EndPoint of an obstacle prefab.
/// When the player reaches this trigger, the obstacle is considered "passed",
/// and the score is updated accordingly.
/// </summary>
public class ObstacleEndTrigger : MonoBehaviour
{
    private bool triggered = false; // Ensures score is only awarded once per obstacle

    /// <summary>
    /// Detects trigger collision with the player to mark obstacle as passed.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger once, and only if the player enters the trigger zone
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true; // Prevent this from triggering again

            // Register the obstacle pass event with the ScoreManager
            ScoreManager.Instance?.RegisterObstaclePassed();

            Debug.Log("✅ Obstacle passed!");
        }
    }
}
