using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the spike's trigger is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("☠️ Player hit spikes — Dead");

            // Destroy the player GameObject
            Destroy(other.gameObject);

            // Optional: Trigger death animation or game over logic here
        }
    }
}