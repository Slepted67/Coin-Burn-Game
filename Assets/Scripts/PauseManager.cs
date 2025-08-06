using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles pausing and resuming the game, along with retrying and returning to the main menu.
/// The Escape key toggles pause, and the pause UI appears or disappears accordingly.
/// </summary>
public class PauseManager : MonoBehaviour
{
    // Static flag to check global pause state
    public static bool IsPaused = false;

    [Header("UI References")]
    public GameObject pauseUI; // UI panel to show when paused

    void Update()
    {
        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void OnEnable()
    {
        // Subscribe to scene load event to reset pause on new scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Clean up event subscription
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Ensures pause state is reset and UI is hidden when a new scene loads.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pauseUI != null)
            pauseUI.SetActive(false);

        Time.timeScale = 1f;
        IsPaused = false;
    }

    /// <summary>
    /// Pauses the game: activates pause UI and freezes time.
    /// </summary>
    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f; // Freeze everything
        IsPaused = true;
    }

    /// <summary>
    /// Resumes the game from pause: hides UI and unfreezes time.
    /// </summary>
    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f; // Resume everything
        IsPaused = false;
    }

    /// <summary>
    /// Reloads the current game scene (retry).
    /// </summary>
    public void Retry()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void MainMenu()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        SceneManager.LoadScene("MainMenuScene");
    }
}
