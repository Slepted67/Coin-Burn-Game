using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int value = 1; // Value added to player's coin count

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCoins player = collision.GetComponent<PlayerCoins>();
            if (player != null)
            {
                player.AddCoins(value);

                // ✅ Notify score manager with combo context
                ScoreManager.Instance?.RegisterCoinPickup(true);

                Destroy(gameObject);
            }
        }
    }
}