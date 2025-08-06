using UnityEngine;
using System.Collections;

public class PlayerWallCollision : MonoBehaviour
{
    [Header("Wall Collision Settings")]
    public float stunDuration = 0.75f;
    public int coinLossMin = 1;
    public int coinLossMax = 3;

    private bool isStunned = false;
    private PlayerMovement movement;
    private PlayerCoins coins;
    private Animator animator;


    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        coins = GetComponent<PlayerCoins>();
        animator = GetComponentInChildren<Animator>(); // Get Animator from the player object
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStunned) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            // Check for side impact (ignore top or bottom bumps)
            if (Mathf.Abs(normal.x) > 0.8f && Mathf.Abs(normal.y) < 0.3f)
            {
                Debug.Log("💥 Side wall hit! Apply penalty");

                StartCoroutine(StunAndLoseCoins());
                break;
            }
        }
    }

    IEnumerator StunAndLoseCoins()
    {
        isStunned = true;
        movement.enabled = false;

        animator.SetTrigger("Hurt"); // Trigger hurt animation

        int coinsToLose = Random.Range(coinLossMin, coinLossMax + 1);
        for (int i = 0; i < coinsToLose; i++)
        {
            coins.DropCoin(); // Assumes DropCoin() already handles coin loss logic
        }

        yield return new WaitForSeconds(stunDuration);

        movement.enabled = true;
        isStunned = false;
    }
}
