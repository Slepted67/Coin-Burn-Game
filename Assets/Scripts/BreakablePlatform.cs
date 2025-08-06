using UnityEngine;

/// <summary>
/// BreakablePlatform controls a platform that collapses shortly after the player lands on it.
/// It triggers when the player lands on it from above, awards a score bonus,
/// and then disables or destroys itself after a short delay.
/// </summary>
public class BreakablePlatform : MonoBehaviour
{
    [Header("Settings")]
    public float breakDelay = 0.5f; // Time (in seconds) before the platform disappears after being stepped on

    private bool used = false; // Ensures the platform only breaks once

    /// <summary>
    /// Called when another collider makes contact with this platform.
    /// Checks if the player landed from above and triggers platform break logic.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (used) return; // Already used, skip

        if (collision.collider.CompareTag("Player"))
        {
            // Get the collision contact point and compare Y to platform center to make sure it's from above
            Vector2 contactPoint = collision.contacts[0].point;
            if (contactPoint.y > transform.position.y)
            {
                used = true;
                Debug.Log("Breakable platform stepped on...");

                // Award score for using the breakable platform
                ScoreManager.Instance?.RegisterBreakablePlatformUse();

                // Delay break action for effect (animation/timing)
                Invoke(nameof(BreakPlatform), breakDelay);
            }
        }
    }

    /// <summary>
    /// Breaks the platform after the delay. Can be extended to include animations or effects.
    /// </summary>
    private void BreakPlatform()
    {
        Debug.Log("Platform broken!");

        // OPTIONAL: Add animation or particle effect here

        // === Removal Options ===
        // Option A: Disable the platform so it can be reused later if needed
        gameObject.SetActive(false);

        // Option B (alternative): Permanently destroy the platform object from the scene
        // Destroy(gameObject);
    }
}
