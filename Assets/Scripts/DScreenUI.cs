using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the Death Screen UI that appears when the player dies.
/// Shows the final score and offers buttons to retry or return to main menu.
/// </summary>
public class DScreenUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathPanel;               // The full death screen panel
    public TextMeshProUGUI finalScoreText;      // Displays final score text

    /// <summary>
    /// Ensures the death panel starts hidden when the scene loads.
    /// </summary>
    private void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false); // Hide on start
    }

    /// <summary>
    /// Activates the death screen and displays the final score.
    /// Called when the player dies.
    /// </summary>
    public void ShowDeathScreen()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true); // Show the panel

        // Get score from ScoreManager (safe fallback to 0 if null)
        int score = ScoreManager.Instance?.score ?? 0;
        finalScoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Reloads the current scene, restarting the run.
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Loads the main menu scene by name.
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
