using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private bool isDying = false;

    public float deathDelay = 1.2f; // Adjust based on animation length

    void Start()
    {
        // Use GetComponentInChildren since Animator is on child object
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TriggerDeath()
    {
        if (isDying) return;
        isDying = true;

        Debug.Log("💀 Triggering death animation");

        // Trigger the death animation
        animator.SetTrigger("Death");

        // Freeze player movement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;


        // Disable player control scripts
        GetComponent<PlayerMovement>().enabled = false;

        // Disable any colliders to prevent further interactions
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        // Start coroutine to wait, then trigger death logic and destroy
        StartCoroutine(DelayedDeath());
    }

    private System.Collections.IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(deathDelay);

        GManager.Instance?.PlayerDied();
    }
}
