using UnityEngine;

public class SpringBounceTrigger : MonoBehaviour
{
    public float bounceForce = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                Debug.Log("🌀 Bounce pad activated!");
            }
        }
    }
}