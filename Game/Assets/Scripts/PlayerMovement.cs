using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundPoundForce = 20f;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGroundPounding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (Input.GetButtonDown("GroundPound") && !isGrounded && !isGroundPounding)
        {
            StartGroundPound();
        }
        if (isGrounded && isGroundPounding)
        {
            StopGroundPound();
        }
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (!isGroundPounding)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void StartGroundPound()
    {
        isGroundPounding = true;
        rb.linearVelocity = new Vector2(0, -groundPoundForce);
    }

    void StopGroundPound()
    {
        isGroundPounding = false;
        rb.linearVelocity = Vector2.zero;
    }


}
