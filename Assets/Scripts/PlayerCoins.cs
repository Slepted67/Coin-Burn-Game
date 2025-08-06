using UnityEngine;

/// <summary>
/// Manages the player's coin count and dynamically adjusts their speed based on that count.
/// Also supports a temporary double-points bonus effect.
/// </summary>
public class PlayerCoins : MonoBehaviour
{
    [Header("Coin & Speed Settings")]
    public int coinCount = 0;           // Total coins the player has collected
    public float baseSpeed = 5f;        // Base movement speed with 0 coins
    public float speedPerCoin = 0.25f;  // Speed gained per collected coin
    public float maxSpeed = 15f;        // Max cap for player speed

    private PlayerMovement movement;    // Reference to the PlayerMovement script for speed control

    [Header("Power-Up: Double Points")]
    private bool doublePointsActive = false;  // Is double point effect active
    private float doublePointsTimer = 0f;     // Timer for how long it stays active

    void Start()
    {
        // Cache reference to movement script and set starting speed
        movement = GetComponent<PlayerMovement>();
        UpdatePlayerSpeed();
    }

    void Update()
    {
        // If double points effect is active, tick down the timer
        if (doublePointsActive)
        {
            doublePointsTimer -= Time.deltaTime;

            // Disable double points when time runs out
            if (doublePointsTimer <= 0f)
            {
                doublePointsActive = false;
            }
        }
    }

    /// <summary>
    /// Adds coins to the player. If double points is active, adds double the amount.
    /// </summary>
    /// <param name="amount">Number of coins to add</param>
    public void AddCoins(int amount)
    {
        int finalAmount = doublePointsActive ? amount * 2 : amount;
        coinCount += finalAmount;

        UpdatePlayerSpeed(); // Reflect coin count change in player speed
        Debug.Log($"Coin collected. Amount: {finalAmount}, Total: {coinCount}");
    }

    /// <summary>
    /// Drops one coin and reduces the player’s speed accordingly.
    /// </summary>
    public void DropCoin()
    {
        if (coinCount > 0)
        {
            coinCount--;
            UpdatePlayerSpeed();
            Debug.Log($"Coin dropped. Total: {coinCount}");
        }
    }

    /// <summary>
    /// Calculates and sets the player’s current target speed based on coins.
    /// </summary>
    private void UpdatePlayerSpeed()
    {
        float newSpeed = baseSpeed + (coinCount * speedPerCoin);
        movement.targetSpeed = Mathf.Min(newSpeed, maxSpeed); // Enforce max speed cap
    }

    /// <summary>
    /// Activates a temporary power-up that doubles coins collected.
    /// </summary>
    /// <param name="duration">Duration in seconds for the effect</param>
    public void ActivateDoublePoints(float duration)
    {
        doublePointsActive = true;
        doublePointsTimer = duration;
        Debug.Log("🎉 Double coin value activated for " + duration + " seconds!");
    }
}