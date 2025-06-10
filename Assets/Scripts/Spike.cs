using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("☠️ Player hit spikes — Dead");
            
            Destroy(other.gameObject);
        }
    }
}
