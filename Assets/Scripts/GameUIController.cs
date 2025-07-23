using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinText;       // Displays current coin count
    public TextMeshProUGUI speedText;      // Displays current movement speed

    [Header("Player References")]
    public PlayerCoins playerCoins;        // Reference to PlayerCoins script
    public PlayerMovement playerMovement;  // Reference to PlayerMovement script

    void Update()
    {
        // Update coin UI if reference is valid
        if (playerCoins != null)
        {
            coinText.text = "Coins: " + playerCoins.coinCount;
        }

        // Update speed UI if reference is valid
        if (playerMovement != null)
        {
            float currentSpeed = Mathf.Abs(playerMovement.GetComponent<Rigidbody2D>().linearVelocity.x);
            speedText.text = "Speed: " + currentSpeed.ToString("F1");
        }
    }
}