using UnityEngine;

/// <summary>
/// A bounce pad that launches the player upwards when landed on from above.
/// </summary>
public class SpringBounceTrigger : MonoBehaviour
{
    [Header("Spring Settings")]
    public float bounceForce = 20f; // Vertical force applied to the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only bounce the player if they're above the spring and tagged correctly
        if (other.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Apply upward bounce
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                Debug.Log("🌀 Bounce pad activated!");

                // ✅ Add points for using the spring pad
                ScoreManager.Instance?.RegisterSpringBounce();
            }
        }
    }
}
