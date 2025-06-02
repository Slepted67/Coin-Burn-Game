using UnityEngine;
using TMPro;

public class SpeedGate : MonoBehaviour
{
    [Header("Speed Requirements")]
    public float requiredSpeed = 8f;
    public bool killOnFail = false;
    public bool bounceOnFail = true;

    private GameObject barrierObject;
    private bool gateChecked = false;

    void Start()
    {
        // Find and deactivate the barrier child
        Transform barrierTransform = transform.parent.Find("GateBarrier");
        if (barrierTransform != null)
        {
            barrierObject = barrierTransform.gameObject;
            barrierObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GateBarrier not found as child of SpeedGate.");
        }

        // Find and update gate speed text
        TextMeshPro speedText = transform.parent.Find("GateSpeed")?.GetComponent<TextMeshPro>();
        if (speedText != null)
        {
            speedText.text = requiredSpeed.ToString("F0");

            // Optional: color code based on difficulty
            if (requiredSpeed >= 10f) speedText.color = Color.red;
            else if (requiredSpeed >= 6f) speedText.color = Color.yellow;
            else speedText.color = Color.green;
        }
        else
        {
            Debug.LogWarning("GateSpeedText not found as child of SpeedGate.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gateChecked) return;
        if (!collision.CompareTag("Player")) return;

        PlayerCoins coinSystem = collision.GetComponent<PlayerCoins>();
        PlayerMovement moveSystem = collision.GetComponent<PlayerMovement>();

        if (coinSystem != null && moveSystem != null)
        {
            float currentSpeed = moveSystem.walkSpeed;

            if (currentSpeed >= requiredSpeed)
            {
                Debug.Log("Gate Passed ✅");
                // Nothing needed — let them through
            }
            else
            {
                Debug.Log("Gate Blocked ❌");

                if (barrierObject != null)
                    barrierObject.SetActive(true);

                if (killOnFail)
                {
                    Destroy(collision.gameObject); // or call a death animation
                }
                else if (bounceOnFail)
                {
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Push player back slightly
                        rb.linearVelocity = new Vector2(-moveSystem.moveDirection * 5f, rb.linearVelocity.y);
                    }
                }
            }

            gateChecked = true;
        }
    }
}