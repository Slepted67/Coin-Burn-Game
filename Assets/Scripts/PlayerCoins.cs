using UnityEngine;

/// Manages the player's coin count and adjusts movement speed based on coins.
public class PlayerCoins : MonoBehaviour
{
    public int coinCount = 0;
    public float baseSpeed = 5f;
    public float speedPerCoin = 0.25f;
    public float maxSpeed = 15f;

    private PlayerMovement movement;

    private bool doublePointsActive = false;
    private float doublePointsTimer = 0f;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        UpdatePlayerSpeed();
    }

    void Update()
    {
        if (doublePointsActive)
        {
            doublePointsTimer -= Time.deltaTime;
            if (doublePointsTimer <= 0f)
            {
                doublePointsActive = false;
            }
        }
    }

    public void AddCoins(int amount)
    {
        int finalAmount = doublePointsActive ? amount * 2 : amount;
        coinCount += finalAmount;
        UpdatePlayerSpeed();
        Debug.Log($"Coin collected. Amount: {finalAmount}, Total: {coinCount}");
    }

    public void DropCoin()
    {
        if (coinCount > 0)
        {
            coinCount--;
            UpdatePlayerSpeed();
            Debug.Log($"Coin dropped. Total: {coinCount}");
        }
    }

    private void UpdatePlayerSpeed()
    {
        float newSpeed = baseSpeed + (coinCount * speedPerCoin);
        movement.targetSpeed = Mathf.Min(newSpeed, maxSpeed);
    }

    // ✅ Call this to activate the bonus
    public void ActivateDoublePoints(float duration)
    {
        doublePointsActive = true;
        doublePointsTimer = duration;
        Debug.Log("🎉 Double coin value activated for " + duration + " seconds!");
    }
}
