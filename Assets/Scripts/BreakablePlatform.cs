using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public float breakDelay = 1.5f; // Seconds before platform breaks

    private bool breaking = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (breaking) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            breaking = true;
            Debug.Log("Breakable platform stepped on...");

            // ✅ Register the platform usage for scoring
            ScoreManager.Instance?.RegisterBreakablePlatformUse();

            Invoke(nameof(BreakPlatform), breakDelay);
        }
    }

    private void BreakPlatform()
    {
        Debug.Log("Platform broken!");

        // TODO: Add shake or fade animation here if desired

        // === Removal Options ===

        // Option A: Disable the platform (still exists but invisible/inactive)
        // gameObject.SetActive(false);

        // Option B: Fully destroy the platform from the scene
        Destroy(gameObject);
    }
}
