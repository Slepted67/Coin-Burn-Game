using UnityEngine;
using TMPro;

/// <summary>
/// Controls in-game UI elements that show coin count and player speed.
/// Continuously updates the UI based on player state.
/// </summary>
public class GameUIController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinText;       // UI text to show current coin count
    public TextMeshProUGUI speedText;      // UI text to show current movement speed

    [Header("Player References")]
    public PlayerCoins playerCoins;        // Reference to PlayerCoins script (tracks coins)
    public PlayerMovement playerMovement;  // Reference to PlayerMovement script (tracks movement)

    /// <summary>
    /// Called every frame to update the coin and speed displays.
    /// </summary>
    void Update()
    {
        // Safely update the coin UI text
        if (playerCoins != null)
        {
            coinText.text = "Coins: " + playerCoins.coinCount;
        }

        // Safely update the speed UI text
        if (playerMovement != null)
        {
            // Get the player's current horizontal speed (absolute value)
            float currentSpeed = Mathf.Abs(playerMovement.GetComponent<Rigidbody2D>().linearVelocity.x);

            // Show speed with 1 decimal precision
            speedText.text = "Speed: " + currentSpeed.ToString("F1");
        }
    }
}
