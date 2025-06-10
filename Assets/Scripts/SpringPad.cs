using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public float bounceForce = 20f;
    private Transform bounceTrigger;

    void Start()
    {
        bounceTrigger = transform.Find("BounceTrigger");
        if (bounceTrigger == null)
        {
            Debug.LogWarning("BounceTrigger not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only respond if this was the BounceTrigger zone
        if (other.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                Debug.Log("Player bounced upward by spring!");
            }
        }
    }
}