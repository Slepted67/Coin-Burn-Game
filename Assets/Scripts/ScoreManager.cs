using UnityEngine;

/// <summary>
/// Manages all scoring mechanics and breakdowns in the game:
/// distance, coins, combo, obstacles, milestones, speed bonuses, and penalties.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;

    [Header("Combo Settings")]
    [SerializeField] private float comboDuration = 1.0f; // How long a combo lasts
    private float comboTimer = 0f;
    private bool comboActive = false;

    [Header("Max Speed Scoring")]
    private float maxSpeedTimer = 0f;          // Time player maintains max speed
    private float maxSpeedCheckRate = 0.1f;    // How often to check

    [Header("Obstacle Tracking")]
    private int obstaclesPassed = 0;
    private int nextMilestone = 10;
    private float lastXPos;
    private Transform playerTransform;

    [Header("Score Breakdown")]
    public int distanceScore = 0;
    public int coinScore = 0;
    public int comboScore = 0;
    public int obstacleScore = 0;
    public int milestoneScore = 0;
    public int springScore = 0;
    public int breakableScore = 0;
    public int speedBonusScore = 0;
    public int coinLossPenalty = 0;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Grab player reference
        playerTransform = GameObject.FindWithTag("Player").transform;
        lastXPos = playerTransform.position.x;

        // Start checking for max speed bonuses
        InvokeRepeating(nameof(CheckMaxSpeed), maxSpeedCheckRate, maxSpeedCheckRate);
    }

    void Update()
    {
        HandleDistanceScore();  // Adds points as player moves forward
        HandleComboTimer();     // Manages combo timing logic
    }

    // === Distance-Based Scoring ===
    void HandleDistanceScore()
    {
        if (playerTransform == null) return;

        float currentX = playerTransform.position.x;
        float delta = currentX - lastXPos;

        if (delta > 0)
        {
            int earned = (int)(delta * 10f);  // 10 points per unit moved right
            AddScore(earned);
            distanceScore += earned;
        }

        lastXPos = currentX;
    }

    // === Max Speed Bonus (every 3 seconds at full speed) ===
    void CheckMaxSpeed()
    {
        if (playerTransform == null) return;

        var player = playerTransform.GetComponent<PlayerMovement>();
        if (player != null && player.walkSpeed >= player.targetSpeed)
        {
            maxSpeedTimer += maxSpeedCheckRate;

            if (maxSpeedTimer >= 3f)
            {
                AddScore(50);
                speedBonusScore += 50;
                maxSpeedTimer = 0f;
                Debug.Log("🚀 Max speed sustained — +50 points!");
            }
        }
        else
        {
            maxSpeedTimer = 0f;
        }
    }

    // === Combo Timer ===
    void HandleComboTimer()
    {
        if (comboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboActive = false;
                Debug.Log("Combo ended");
            }
        }
    }

    // === Called by Coin.cs ===
    public void RegisterCoinPickup(bool isComboEligible = true)
    {
        AddScore(15);
        coinScore += 15;
        Debug.Log("💰 Coin picked up: +15");

        if (comboActive && isComboEligible)
        {
            AddScore(5);
            comboScore += 5;
            Debug.Log("🔥 Combo coin: +5 bonus");
        }

        comboTimer = comboDuration;
        comboActive = true;
    }

    // === Called by ObstacleEndTrigger.cs ===
    public void RegisterObstaclePassed()
    {
        obstaclesPassed++;
        AddScore(100);
        obstacleScore += 100;
        Debug.Log($"✅ Obstacle passed! Total: {obstaclesPassed}");

        // Bonus for hitting milestone (every 10 obstacles)
        if (obstaclesPassed >= nextMilestone)
        {
            AddScore(100);
            milestoneScore += 100;
            Debug.Log("🎉 Milestone reached — +100 bonus");
            nextMilestone += 10;
        }
    }

    // === Called by SpringPad.cs ===
    public void RegisterSpringBounce()
    {
        AddScore(25);
        springScore += 25;
        Debug.Log("🌀 Spring bounce: +25");
    }

    // === Called by BreakablePlatform.cs ===
    public void RegisterBreakablePlatformUse()
    {
        AddScore(50);
        breakableScore += 50;
        Debug.Log("🪵 Breakable platform used: +50");
    }

    // === Called when player drops coins ===
    public void RegisterCoinLoss(int count)
    {
        int penalty = count * 10;
        AddScore(-penalty);
        coinLossPenalty += penalty;
        Debug.Log($"💥 Lost {count} coins — -{penalty} points");
    }

    // === Centralized method to update total score ===
    public void AddScore(int amount)
    {
        score += amount;
    }

    // === Saves score to PlayerPrefs with index ===
    public void SaveRunScore()
    {
        int currentScore = score;
        int runCount = PlayerPrefs.GetInt("RunCount", 0);

        PlayerPrefs.SetInt($"RunScore_{runCount}", currentScore);
        PlayerPrefs.SetInt("RunCount", runCount + 1);
        PlayerPrefs.Save();

        Debug.Log($"✅ Saved run {runCount} with score {currentScore}");
    }
}
