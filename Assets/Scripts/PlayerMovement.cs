using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float jumpForce = 10f;
    public float slideDuration = 0.5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    [Header("Sprite Flip")]
    public Transform bodySprite;

    [Header("Acceleration")]
    public float accelerationRate = 5f;
    public float acceleration = 30f;
    public float deceleration = 40f;
    public float targetSpeed;
    private float currentSpeedX = 0f;

    [Header("Animation")]
    // At the top of both scripts
    private Animator animator;
    private bool wasRunning = false;

    // ----------------------------
    // === Runtime States ===
    // ----------------------------

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isSliding = false;
    private float slideTimer = 0f;
    private bool autoRunEnabled = false;
    public int moveDirection { get; private set; } = 1;  // 1 = right, -1 = left
    private float burnCooldown = 1f;        // cooldown time in seconds
    private float burnCooldownTimer = 0f;   // current cooldown time

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();

        targetSpeed = walkSpeed;
    }

    void Update()
    {
        // --- Burn Coin Cooldown ---
        if (burnCooldownTimer > 0f)
            burnCooldownTimer -= Time.deltaTime;

        // --- Coin Burn ---
        if (Input.GetKeyDown(KeyCode.F) && burnCooldownTimer <= 0f)
        {
            PlayerCoins coinSystem = GetComponent<PlayerCoins>();
            if (coinSystem != null && coinSystem.coinCount > 0)
            {
                coinSystem.DropCoin();
                ScoreManager.Instance?.RegisterCoinLoss(1);
                Debug.Log("🔥 Coin burned by player!");
                burnCooldownTimer = burnCooldown;
            }
            else
            {
                Debug.Log("⚠️ No coins to burn.");
            }
        }

        // --- Toggle autorun with 'R' ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Autorun Toggled");
            autoRunEnabled = !autoRunEnabled;
        }

        // --- Ground Check ---
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - 1f);
        isGrounded = Physics2D.OverlapCircle(checkPos, 0.1f, groundLayer);

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jumped");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Slide (Dash) input and animation
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isSliding)
        {
            Debug.Log("Slid");
            isSliding = true;
            slideTimer = slideDuration;
            rb.linearVelocity = new Vector2(moveDirection * walkSpeed * 1.5f, rb.linearVelocity.y);
            animator.SetTrigger("Dash"); // 🌀 Trigger dash animation here
        }

        // Slide timer expiration
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
                isSliding = false;
        }

        // Animation: Jumping and Falling
        bool isRising = rb.linearVelocity.y > 0.1f;
        bool isFalling = rb.linearVelocity.y < -0.1f;

        animator.SetBool("isJumping", isRising && !isGrounded);
        animator.SetBool("isFalling", isFalling && !isGrounded);


        // Animation: Running
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        float currentHorizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        bool isNowStopped = currentHorizontalSpeed <= 0.1f;
        bool wasMovingBefore = wasRunning;

        if (wasMovingBefore && isNowStopped && isGrounded && !isSliding)
        {
            animator.SetTrigger("Stop");
        }

        // Update wasRunning for next frame
        wasRunning = currentHorizontalSpeed > 0.1f;



        // Flip sprite
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

        // --- Horizontal Movement Input ---
        if (Input.GetKey(KeyCode.A))
        {
            // Debug.Log("Move Left");
            horizontal = -1;
            moveDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log("Move Right");
            horizontal = 1;
            moveDirection = 1;
        }
        else if (autoRunEnabled)
        {
            horizontal = moveDirection;
        }

        // --- Gradually shift walkSpeed toward targetSpeed ---
        walkSpeed = Mathf.MoveTowards(walkSpeed, targetSpeed, accelerationRate * Time.fixedDeltaTime);

        if (!isSliding)
        {
            float targetVelocityX = horizontal * walkSpeed;

            // Accelerate or decelerate based on input
            if (horizontal != 0)
            {
                currentSpeedX = Mathf.MoveTowards(currentSpeedX, targetVelocityX, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeedX = Mathf.MoveTowards(currentSpeedX, 0, deceleration * Time.fixedDeltaTime);
            }
            // Apply final velocity
            rb.linearVelocity = new Vector2(currentSpeedX, rb.linearVelocity.y);
        }
    }

    // --- Visualize Ground Check in Scene View ---
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - 1f);
        Gizmos.DrawWireSphere(checkPos, 0.1f);
    }
    
    public void ForceJumpAnimation()
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", false);
    }
}