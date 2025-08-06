using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks and saves recent run scores across scenes.
/// Stores the last 15 runs using PlayerPrefs and persists between scenes.
/// </summary>
public class RunScoreTracker : MonoBehaviour
{
    public static RunScoreTracker Instance;

    public List<int> recentScores = new List<int>();  // Stores recent run scores
    private const int MaxScores = 15;                  // Max number of scores to remember

    void Awake()
    {
        // Singleton pattern to ensure only one instance persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction on scene change
            LoadScores();                  // Load saved scores on startup
            Debug.Log("✅ RunScoreTracker is alive across scenes");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    /// <summary>
    /// Adds a new score to the list and saves to PlayerPrefs.
    /// </summary>
    /// <param name="score">The score to add</param>
    public void AddNewScore(int score)
    {
        recentScores.Insert(0, score); // Add new score to the front of the list

        // If list exceeds max, remove the oldest
        if (recentScores.Count > MaxScores)
            recentScores.RemoveAt(recentScores.Count - 1);

        SaveScores(); // Persist changes
    }

    /// <summary>
    /// Saves recent scores to PlayerPrefs for persistence.
    /// </summary>
    void SaveScores()
    {
        for (int i = 0; i < recentScores.Count; i++)
        {
            PlayerPrefs.SetInt("RunScore_" + i, recentScores[i]);
        }

        // Save count so we know how many scores to load later
        PlayerPrefs.SetInt("RunScore_Count", recentScores.Count);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads scores from PlayerPrefs into memory.
    /// </summary>
    void LoadScores()
    {
        recentScores.Clear();
        int count = PlayerPrefs.GetInt("RunScore_Count", 0);

        for (int i = 0; i < count; i++)
        {
            int score = PlayerPrefs.GetInt("RunScore_" + i, 0);
            recentScores.Add(score);
        }
    }
}
