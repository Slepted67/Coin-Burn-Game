using UnityEngine;

/// <summary>
/// Game manager singleton that handles player death and connects with death UI and score systems.
/// </summary>
public class GManager : MonoBehaviour
{
    public static GManager Instance; // Singleton instance for global access

    [Header("References")]
    public GameObject deathScreenUI; // Reference to the death screen UI GameObject

    /// <summary>
    /// Ensures singleton setup and links the death screen UI if not already assigned.
    /// </summary>
    private void Awake()
    {
        // Singleton pattern enforcement
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Try to auto-link DeathScreenUI from the scene if it's not already assigned
        if (deathScreenUI == null)
        {
            deathScreenUI = GameObject.Find("DeathScreenUI");
            if (deathScreenUI != null)
            {
                Debug.Log("✅ Linked deathScreenUI from scene.");
                
                // Check if the component is present for functionality
                var screenScript = deathScreenUI.GetComponent<DScreenUI>();
                if (screenScript == null)
                {
                    Debug.LogError("❌ deathScreenUI found but is missing the DScreenUI component!");
                }
            }
            else
            {
                Debug.LogError("❌ Could not find DeathScreenUI!");
            }
        }
    }

    /// <summary>
    /// Called when the player dies. Triggers score saving and death screen display.
    /// </summary>
    public void PlayerDied()
    {
        Debug.Log("☠️ Player died");

        // If a RunScoreTracker exists, attempt to save the score
        if (RunScoreTracker.Instance != null)
        {
            if (ScoreManager.Instance != null)
            {
                RunScoreTracker.Instance.AddNewScore(ScoreManager.Instance.score);
                ScoreManager.Instance.SaveRunScore(); // Optional: persist score if needed
            }
            else
            {
                Debug.LogError("❌ ScoreManager.Instance is NULL!");
            }
        }
        else
        {
            Debug.LogError("❌ RunScoreTracker.Instance is NULL!");
        }

        // Fallback UI link check in case something was unlinked during gameplay
        if (deathScreenUI == null)
        {
            deathScreenUI = GameObject.Find("DeathScreenUI");
            if (deathScreenUI != null)
            {
                Debug.Log("✅ Linked deathScreenUI from scene.");
                var screenScript = deathScreenUI.GetComponent<DScreenUI>();
                if (screenScript == null)
                {
                    Debug.LogError("❌ deathScreenUI found but is missing the DScreenUI component!");
                }
            }
            else
            {
                Debug.LogError("❌ Could not find DeathScreenUI!");
            }
        }

        // Show the death screen
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
            deathScreenUI.GetComponent<DScreenUI>()?.ShowDeathScreen();
        }
    }
}
