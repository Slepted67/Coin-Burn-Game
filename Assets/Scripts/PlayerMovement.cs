using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float jumpForce = 10f;
    public float slideDuration = 0.5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    [Header("Sprite Flip")]
    public Transform bodySprite; // Assign Plyr_Body in Inspector

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSliding = false;
    private float slideTimer = 0f;

    private bool autoRunEnabled = false;
    public int moveDirection { get; private set; } = 1; // 1 = right, -1 = left

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // - TOGGLE AUTORUN -
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Autorun Toggled");
            autoRunEnabled = !autoRunEnabled;
        }

        // - GROUND CHECK -
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        isGrounded = Physics2D.OverlapCircle(checkPos, 0.1f, groundLayer);

        // - DROP COIN -
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Coin Burned");
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerCoins coinSystem = GetComponent<PlayerCoins>();
                if (coinSystem != null)
                {
                    coinSystem.DropCoin();
                }
            }

        }

        // - JUMP -
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jumped");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // - SLIDE -
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isSliding)
        {
            Debug.Log("Slide Baby");
            isSliding = true;
            slideTimer = slideDuration;
            rb.linearVelocity = new Vector2(moveDirection * walkSpeed * 1.5f, rb.linearVelocity.y);
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
                isSliding = false;
        }

        // - SPRITE FLIP -
        if (moveDirection != 0 && bodySprite != null)
        {
            Vector3 scale = bodySprite.localScale;
            scale.x = Mathf.Abs(scale.x) * moveDirection;
            bodySprite.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        float horizontal = 0;

        // - Movement Input -
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Move Left");
            horizontal = -1;
            moveDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Move Right");
            horizontal = 1;
            moveDirection = 1;
        }
        else if (autoRunEnabled)
        {
            horizontal = moveDirection; // continue auto-running
        }

        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(horizontal * walkSpeed, rb.linearVelocity.y);
        }
    }
}
