using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ScoreScroller : MonoBehaviour
{
    [Header("UI References")]
    public GameObject scoreEntryPrefab;
    public Transform contentPanel;
    public ScrollRect scrollRect;

    [Header("Scrolling Settings")]
    public float scrollSpeed = 20f;
    public float pauseAtTop = 1f;
    public float pauseAtBottom = 1f;

    [Header("Fake Score Settings")]
    public int totalRunsToShow = 15;

    private float scrollHeight;
    private RectTransform contentRect;

    void Start()
    {
        contentRect = contentPanel.GetComponent<RectTransform>();
        PopulateScores();
        StartCoroutine(AutoScroll());
    }

    void PopulateScores()
    {
        List<(int runNumber, int score)> scores = new List<(int, int)>();

        int runCount = PlayerPrefs.GetInt("RunCount", 0);

        for (int i = 0; i < runCount; i++)
        {
            int score = PlayerPrefs.GetInt($"RunScore_{i}", 0);
            scores.Add((i, score)); // Run i, Score
        }

        // Sort scores by highest to lowest
        var sorted = scores.OrderByDescending(s => s.score).ToList();

        // Take top 15
        foreach (var entryData in sorted.Take(totalRunsToShow))
        {
            GameObject entry = Instantiate(scoreEntryPrefab, contentPanel);
            entry.GetComponent<TMP_Text>().text = $"Run {entryData.runNumber}: {entryData.score}";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        scrollHeight = contentRect.rect.height - scrollRect.viewport.rect.height;
    }


    IEnumerator AutoScroll()
    {
        yield return new WaitForSeconds(pauseAtTop);
        while (true)
        {
            float scrollPos = 0f;

            while (scrollPos < scrollHeight)
            {
                scrollPos += scrollSpeed * Time.deltaTime;
                float normalizedPos = Mathf.Clamp01(scrollPos / scrollHeight);
                scrollRect.verticalNormalizedPosition = 1f - normalizedPos;
                yield return null;
            }

            yield return new WaitForSeconds(pauseAtBottom);
            scrollRect.verticalNormalizedPosition = 1f;
            yield return new WaitForSeconds(pauseAtTop);
        }
    }
}
