using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public int coinCount = 0;
    public float baseSpeed = 5f;
    public float speedPerCoin = 0.25f;
    public float maxSpeed = 15f;

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        UpdatePlayerSpeed();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdatePlayerSpeed();
        Debug.Log($"Coin collected. Total: {coinCount}");
    }

    public void DropCoin()
    {
        if (coinCount > 0)
        {
            coinCount--;
            UpdatePlayerSpeed();
            Debug.Log($"Coin dropped. Total: {coinCount}");
            // TODO: Spawn dropped coin if needed
        }
    }

    private void UpdatePlayerSpeed()
    {
        float newSpeed = baseSpeed + (coinCount * speedPerCoin);
        movement.walkSpeed = Mathf.Min(newSpeed, maxSpeed);
    }
}