using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float breakDelay = 1.5f; // Seconds before platform breaks
    private bool breaking = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (breaking) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            breaking = true;
            Debug.Log("Breakable platform stepped on...");
            Invoke(nameof(BreakPlatform), breakDelay);
        }
    }

    private void BreakPlatform()
    {
        Debug.Log("Platform broken!");
        
        // Visual fade/shake later

        // Option A: disable platform
        // gameObject.SetActive(false);

        // Option B: destroy platform
        Destroy(gameObject);
    }
}