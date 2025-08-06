using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the main menu UI, including scene transitions and settings like volume, resolution, and fullscreen.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;    // Main menu buttons
    public GameObject settingsPanel;    // Settings UI

    [Header("Settings")]
    public Slider volumeSlider;         // Slider to control volume
    public TMP_Dropdown resolutionDropdown; // Dropdown to choose screen resolution
    public Toggle fullscreenToggle;     // Toggle for fullscreen/windowed mode

    private void Start()
    {
        // Show main menu and hide settings by default
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        // Load saved volume setting (defaults to 1.0 if not set)
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = volume;
        AudioListener.volume = volume;

        // Load and apply resolution and dropdown
        PopulateResolutionDropdown();
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 8); // Default to 1920x1080
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        OnResolutionChanged(savedIndex); // Apply resolution

        // Load and apply fullscreen toggle
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// Populates the dropdown with preset resolution options.
    /// </summary>
    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>()
        {
            "3840 x 2160",
            "2560 x 2048",
            "2560 x 1920",
            "2560 x 1600",
            "2048 x 1536",
            "2048 x 1152",
            "1920 x 1440",
            "1920 x 1200",
            "1920 x 1080"
        };

        resolutionDropdown.AddOptions(options);
    }

    /// <summary>
    /// Loads the main gameplay scene.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Loads test scene (disabled but ready for use).
    /// </summary>
    public void StartTestMode()
    {
        // SceneManager.LoadScene("TestScene"); // optional test scene
    }

    /// <summary>
    /// Opens the settings panel.
    /// </summary>
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the settings panel.
    /// </summary>
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    /// <summary>
    /// Exits the game (does nothing in the editor).
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    /// <summary>
    /// Called when volume slider is adjusted. Updates volume and saves it.
    /// </summary>
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes screen resolution based on dropdown selection.
    /// </summary>
    public void OnResolutionChanged(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();

        // Manually apply resolution from dropdown index
        switch (index)
        {
            case 0: Screen.SetResolution(3840, 2160, false); break;
            case 1: Screen.SetResolution(2560, 2048, false); break;
            case 2: Screen.SetResolution(2560, 1920, false); break;
            case 3: Screen.SetResolution(2560, 1600, false); break;
            case 4: Screen.SetResolution(2048, 1536, false); break;
            case 5: Screen.SetResolution(2048, 1152, false); break;
            case 6: Screen.SetResolution(1920, 1440, false); break;
            case 7: Screen.SetResolution(1920, 1200, false); break;
            case 8: Screen.SetResolution(1920, 1080, false); break;
        }
    }

    /// <summary>
    /// Toggles fullscreen mode and re-applies resolution if disabled.
    /// </summary>
    public void OnFullscreenToggle(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0);
        PlayerPrefs.Save();

        // When leaving fullscreen, ensure resolution is still correct
        if (!isOn)
        {
            OnResolutionChanged(resolutionDropdown.value);
        }
    }
}