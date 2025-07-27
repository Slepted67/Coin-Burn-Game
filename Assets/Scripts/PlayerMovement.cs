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
    private Animator animator;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool isSliding = false;
    private float slideTimer = 0f;

    private bool autoRunEnabled = false;
    public int moveDirection { get; private set; } = 1; // 1 = right, -1 = left
    [HideInInspector] public float targetSpeed;
    public float accelerationRate = 5f;  // Adjust this to feel snappy or smooth

    [Header("Acceleration")]
    public float acceleration = 30f;
    public float deceleration = 40f;

    private float currentSpeedX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetSpeed = walkSpeed;
        wasGroundedLastFrame = true;
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
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - 1f);
        isGrounded = Physics2D.OverlapCircle(checkPos, 0.1f, groundLayer);

        // - LANDING LOGIC -
        if (isGrounded && !wasGroundedLastFrame)
        {
            Debug.Log("Landed");
            animator.SetTrigger("Landed");
        }

        // Update grounded state to animator 
        animator.SetBool("isGrounded", isGrounded);
        wasGroundedLastFrame = isGrounded;

        // - DROP COIN -
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Coin Burned");
            PlayerCoins coinSystem = GetComponent<PlayerCoins>();
            if (coinSystem != null)
            {
                coinSystem.DropCoin();
            }
        }

        // - JUMP -
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jumped");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("JumpTrigger");
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
        // Update animator parameters 

        // Running:
            bool isRunning = Mathf.Abs(currentSpeedX) > 0.1f;
            animator.SetBool("isRunning", isRunning);

         // Sliding: 
            animator.SetBool("isSliding", isSliding);

        
    }

    void FixedUpdate()
    {
        float horizontal = 0;

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
            horizontal = moveDirection;
        }

        // Smooth walkSpeed toward targetSpeed
        walkSpeed = Mathf.MoveTowards(walkSpeed, targetSpeed, accelerationRate * Time.fixedDeltaTime);

        if (!isSliding)
        {
            float targetVelocityX = horizontal * walkSpeed;

            if (horizontal != 0)
            {
                currentSpeedX = Mathf.MoveTowards(currentSpeedX, targetVelocityX, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeedX = Mathf.MoveTowards(currentSpeedX, 0, deceleration * Time.fixedDeltaTime);
            }

            rb.linearVelocity = new Vector2(currentSpeedX, rb.linearVelocity.y);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - 1f);
        Gizmos.DrawWireSphere(checkPos, 0.1f);
    }
}
