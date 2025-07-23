using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;

    private float maxSpeedTimer = 0f;
    private float maxSpeedCheckRate = 0.1f;

    private float lastXPos;
    private int obstacleCounter = 0;

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
        float currentX = GameObject.FindWithTag("Player").transform.position.x;
        float delta = currentX - lastXPos;

        if (delta > 0)
            AddScore((int)(delta * 10f));

        lastXPos = currentX;
    }

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
            }
        }
        else
        {
            maxSpeedTimer = 0f;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        // TODO: Hook UI here later
    }

    public void RegisterObstaclePassed()
    {
        obstacleCounter++;
        if (obstacleCounter % 10 == 0)
        {
            AddScore(100);
            Debug.Log("🎯 Milestone: 10 obstacles cleared!");
        }
    }

    public void RegisterBreakablePlatformUse()
    {
        AddPoints(50); // +50 for choosing the parkour path
        Debug.Log("Scored +50 for breakable platform use.");
    }


    public void RegisterSpringBounce() => AddScore(25);

    public void RegisterBreakableUsed() => AddScore(50);

    public void RegisterComboCoin() => AddScore(5);

    public void RegisterCoinLoss(int count) => AddScore(-10 * count);
}
private float comboTimer = 0f;
private bool comboActive = false;

// How long you have to collect another coin to continue the combo
[SerializeField] private float comboDuration = 1.0f;

public void RegisterCoinPickup(bool isComboEligible = false)
{
    AddPoints(15); // Regular coin value
    Debug.Log("Coin picked up: +15");

    if (comboActive && isComboEligible)
    {
        AddPoints(5); // Bonus for combo
        Debug.Log("Combo coin collected: +5 bonus");
    }

    // Restart or extend combo timer
    comboTimer = comboDuration;
    comboActive = true;
}

private void Update()
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

private int obstaclesPassed = 0;
private int nextMilestone = 10;

public void RegisterObstaclePassed()
{
    obstaclesPassed++;
    Debug.Log($"Obstacle passed! Total: {obstaclesPassed}");

    if (obstaclesPassed >= nextMilestone)
    {
        AddPoints(100);
        Debug.Log("🎉 Milestone! +100 points");

        nextMilestone += 10; // Next reward at 20, then 30, etc.
    }
}
