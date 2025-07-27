using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCoins player = collision.GetComponent<PlayerCoins>();
            if (player != null)
            {
                player.AddCoins(value);
                Destroy(gameObject);
            }
        }
    }
}