using UnityEngine;

/// <summary>
/// Coin handles player interaction with collectible coins in the level.
/// When the player touches the coin, it adds value to their coin count,
/// notifies the ScoreManager (for combo handling), and then destroys itself.
/// </summary>
public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int value = 1; // Amount of coins added when collected

    /// <summary>
    /// Triggered when another collider enters the coin's trigger zone.
    /// Used to detect the player and reward coins.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only respond to player collisions
        if (collision.CompareTag("Player"))
        {
            // Try to get the PlayerCoins component from the player
            PlayerCoins player = collision.GetComponent<PlayerCoins>();
            if (player != null)
            {
                // Increase the player's coin count
                player.AddCoins(value);

                // Notify the score system (assumes coin pickup counts toward combo bonus)
                ScoreManager.Instance?.RegisterCoinPickup(true);

                // Remove coin from the scene after collection
                Destroy(gameObject);
            }
        }
    }
}