using UnityEngine;
using TMPro;

/// <summary>
/// A gate that only allows the player to pass through if they are moving at or above a required speed.
/// </summary>
public class SpeedGate : MonoBehaviour
{
    [Header("Speed Requirements")]
    public float requiredSpeed = 8f;      // Speed needed to pass the gate
    public bool killOnFail = false;
    public bool bounceOnFail = true;

    private GameObject barrierObject;
    private bool gateChecked = false;     // Prevents repeated checking

    void Start()
    {
        // Locate and disable the barrier object (child of parent object)
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

        // Set the speed text display (child of parent object)
        TextMeshPro speedText = transform.parent.Find("GateSpeed")?.GetComponent<TextMeshPro>();
        if (speedText != null)
        {
            speedText.text = requiredSpeed.ToString("F0");

            // Optional color coding for visual difficulty cue
            if (requiredSpeed >= 10f)
                speedText.color = Color.red;
            else if (requiredSpeed >= 6f)
                speedText.color = Color.yellow;
            else
                speedText.color = Color.green;
        }
        else
        {
            Debug.LogWarning("GateSpeedText not found as child of SpeedGate.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gateChecked) return;                     // Prevent multiple checks
        if (!collision.CompareTag("Player")) return; // Only allow player to trigger

        PlayerCoins coinSystem = collision.GetComponent<PlayerCoins>();
        PlayerMovement moveSystem = collision.GetComponent<PlayerMovement>();

        if (coinSystem != null && moveSystem != null)
        {
            float currentSpeed = moveSystem.walkSpeed;

            if (currentSpeed >= requiredSpeed)
            {
                Debug.Log("Gate Passed ✅");
                // Let player pass
                coinSystem.ActivateDoublePoints(3f);
            }
            else
            {
                Debug.Log("Gate Blocked ❌");

                // Activate the barrier object
                if (barrierObject != null)
                    barrierObject.SetActive(true);

                if (killOnFail)
                {
                    // Optional: trigger death sequence instead
                    Destroy(collision.gameObject);
                }
                else if (bounceOnFail)
                {
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Bounce the player back
                        rb.linearVelocity = new Vector2(-moveSystem.moveDirection * 5f, rb.linearVelocity.y);
                    }
                }
            }

            gateChecked = true; // Avoid repeat interactions
        }
    }
}
