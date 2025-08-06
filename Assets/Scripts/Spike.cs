using UnityEngine;

public class Spike : MonoBehaviour
{
    // Triggered when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("☠️ Player hit spikes — Dead");

            // Get the PlayerDeathHandler component from the player
            PlayerDeathHandler death = other.GetComponent<PlayerDeathHandler>();
            if (death != null)
            {
                // Trigger the player's death logic
                death.TriggerDeath();
            }
        }
    }
}