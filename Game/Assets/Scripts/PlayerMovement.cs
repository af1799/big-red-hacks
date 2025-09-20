using Unity.VisualScripting;
using UnityEditor.SearchService;
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

    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float lastDashTime = -Mathf.Infinity;
    private float dashDirection = 0f;


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
        if (Input.GetButtonDown("Dash") && !isGroundPounding && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
        if (Input.GetButtonDown("Keyboard"))
        {
            SceneController.Instance.LoadSceneByName("Keyboard");
        }
        if (Input.GetButtonDown("Drums"))
        {
            SceneController.Instance.LoadSceneByName("Drums");
        }
        if (Input.GetButtonDown("Guitar"))
        {
            SceneController.Instance.LoadSceneByName("Guitar");
        }
        if (Input.GetButtonDown("Replay") && !AudioRecorder.Instance.isReplaying)
        {
            AudioRecorder.Instance.PlayAudio();
        } 
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
        }
        else if (!isGroundPounding)
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

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        // Dash direction based on current horizontal input; if zero, dash right by default
        float moveInput = Input.GetAxisRaw("Horizontal");
        dashDirection = moveInput != 0 ? Mathf.Sign(moveInput) : 1f;
    }

    void EndDash()
    {
        isDashing = false;
    }


}
