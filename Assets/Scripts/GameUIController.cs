using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI speedText;
    public PlayerCoins playerCoins;
    public PlayerMovement playerMovement;

    void Update()
    {
        if (playerCoins != null)
        {
            coinText.text = "Coins: " + playerCoins.coinCount;
        }

        if (playerMovement != null)
        {
            speedText.text = "Speed: " + playerMovement.walkSpeed.ToString("F1");
        }
    }
}