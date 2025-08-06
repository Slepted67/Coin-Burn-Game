using UnityEngine;
using TMPro;

/// <summary>
/// A gate that only allows the player to pass through if they are moving at or above a required speed.
/// </summary>
public class SpeedGate : MonoBehaviour
{
    [Header("Speed Requirements")]
    public float requiredSpeed = 8f; // Default, will get overridden dynamically
    public bool killOnFail = false;  // If true, player dies on failure
    public bool bounceOnFail = true; // If true, player is pushed back

    private GameObject barrierObject; // Gate visual that shows up if blocked
    private bool gateChecked = false; // Prevents checking more than once

    void Start()
    {
        // === Dynamically calculate requiredSpeed based on player's max speed ===
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            float cap = player.targetSpeed;
            float min = Mathf.Max(1f, cap - 5f);   // Avoid going too low
            float max = cap + 5f;                  // Adds some challenge variability

            requiredSpeed = Random.Range(min, max); // Pick a random speed
            requiredSpeed = Mathf.Round(requiredSpeed * 10f) / 10f; // Round to 1 decimal place
        }
        else
        {
            Debug.LogWarning("❗ PlayerMovement not found. Using default requiredSpeed.");
        }

        // === Locate the GateBarrier (usually an invisible child GameObject that appears when blocked) ===
        Transform barrierTransform = transform.parent.Find("GateBarrier");
        if (barrierTransform != null)
        {
            barrierObject = barrierTransform.gameObject;
            barrierObject.SetActive(false); // Hide it initially
        }
        else
        {
            Debug.LogWarning("GateBarrier not found as child of SpeedGate.");
        }

        // === Update the speed display text visually on the gate ===
        TextMeshPro speedText = transform.parent.Find("GateSpeed")?.GetComponent<TextMeshPro>();
        if (speedText != null)
        {
            speedText.text = requiredSpeed.ToString("F0"); // Just show whole number

            // Visual cue: Red for hard, Yellow for moderate, Green for easy
            if (requiredSpeed >= 30f)
                speedText.color = Color.red;
            else if (requiredSpeed >= 15f)
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
        if (gateChecked || !collision.CompareTag("Player")) return;

        // Get player's coin and movement systems
        PlayerCoins coinSystem = collision.GetComponent<PlayerCoins>();
        PlayerMovement moveSystem = collision.GetComponent<PlayerMovement>();

        if (coinSystem != null && moveSystem != null)
        {
            float currentSpeed = moveSystem.walkSpeed;

            // ✅ Success! Player passes
            if (currentSpeed >= requiredSpeed)
            {
                Debug.Log("Gate Passed ✅");
                coinSystem.ActivateDoublePoints(3f); // Reward
            }
            else
            {
                // ❌ Fail! Block them
                Debug.Log("Gate Blocked ❌");

                if (barrierObject != null)
                    barrierObject.SetActive(true); // Show the barrier object

                // If killOnFail is true, trigger death
                if (killOnFail)
                {
                    PlayerDeathHandler death = collision.GetComponent<PlayerDeathHandler>();
                    death?.TriggerDeath(); // Safe null check
                }
                // Otherwise, bounce the player back
                else if (bounceOnFail)
                {
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Push back in the opposite direction of movement
                        rb.linearVelocity = new Vector2(-moveSystem.moveDirection * 5f, rb.linearVelocity.y);
                    }
                }
            }

            gateChecked = true; // Ensure this gate is only checked once
        }
    }
}