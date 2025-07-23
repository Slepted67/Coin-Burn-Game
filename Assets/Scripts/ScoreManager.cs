using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;

    [Header("Combo Settings")]
    [SerializeField] private float comboDuration = 1.0f;
    private float comboTimer = 0f;
    private bool comboActive = false;

    [Header("Max Speed Scoring")]
    private float maxSpeedTimer = 0f;
    private float maxSpeedCheckRate = 0.1f;

    [Header("Obstacle Tracking")]
    private int obstaclesPassed = 0;
    private int nextMilestone = 10;

    private float lastXPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        lastXPos = GameObject.FindWithTag("Player").transform.position.x;
        InvokeRepeating(nameof(CheckMaxSpeed), maxSpeedCheckRate, maxSpeedCheckRate);
    }

    void Update()
    {
        HandleDistanceScore();
        HandleComboTimer();
    }

    // -----------------------------------
    // ✅ Distance Traveled Scoring
    // -----------------------------------
    void HandleDistanceScore()
    {
        float currentX = GameObject.FindWithTag("Player").transform.position.x;
        float delta = currentX - lastXPos;

        if (delta > 0)
            AddScore((int)(delta * 10f));

        lastXPos = currentX;
    }

    // -----------------------------------
    // ✅ Max Speed Bonus Scoring
    // -----------------------------------
    void CheckMaxSpeed()
    {
        var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerMovement>();
        if (player != null && player.walkSpeed >= player.targetSpeed)
        {
            maxSpeedTimer += maxSpeedCheckRate;
            if (maxSpeedTimer >= 3f)
            {
                AddScore(50);
                maxSpeedTimer = 0f;
                Debug.Log("🚀 Max speed sustained — +50 points!");
            }
        }
        else
        {
            maxSpeedTimer = 0f;
        }
    }

    // -----------------------------------
    // ✅ Combo Coin System
    // -----------------------------------
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

    public void RegisterCoinPickup(bool isComboEligible = false)
    {
        AddScore(15); // Regular coin
        Debug.Log("💰 Coin picked up: +15");

        if (comboActive && isComboEligible)
        {
            AddScore(5); // Combo bonus
            Debug.Log("🔥 Combo coin: +5 bonus");
        }

        comboTimer = comboDuration;
        comboActive = true;
    }

    // -----------------------------------
    // ✅ Obstacle Tracking
    // -----------------------------------
    public void RegisterObstaclePassed()
    {
        obstaclesPassed++;
        AddScore(100);
        Debug.Log($"✅ Obstacle passed! Total: {obstaclesPassed}");

        if (obstaclesPassed >= nextMilestone)
        {
            AddScore(100); // Milestone bonus
            Debug.Log("🎉 Milestone reached — +100 bonus");
            nextMilestone += 10;
        }
    }

    // -----------------------------------
    // ✅ Other Event Scores
    // -----------------------------------
    public void RegisterSpringBounce()
    {
        AddScore(25);
        Debug.Log("🌀 Spring bounce: +25");
    }

    public void RegisterBreakablePlatformUse()
    {
        AddScore(50);
        Debug.Log("🪵 Breakable platform used: +50");
    }

    public void RegisterCoinLoss(int count)
    {
        int penalty = count * 10;
        AddScore(-penalty);
        Debug.Log($"💥 Lost {count} coins — -{penalty} points");
    }

    // -----------------------------------
    // ✅ Shared Scoring Method
    // -----------------------------------
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("🏆 Score: " + score);
        // TODO: Hook into UI display
    }
}
