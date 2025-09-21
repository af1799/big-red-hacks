using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundPoundForce = 20f;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Spin spin;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGroundPounding = false;
    private bool isPaused = false;

    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float lastDashTime = -Mathf.Infinity;
    private float dashDirection = 0f;

    private GameObject pauseMenuUI;
    private Button restartButton;
    private Button quitButton;

    void Awake()
    {
        pauseMenuUI = GameObject.Find("PauseMenu");
        restartButton = pauseMenuUI.transform.Find("RestartButton").GetComponent<Button>();
        quitButton = pauseMenuUI.transform.Find("QuitButton").GetComponent<Button>();

        pauseMenuUI.SetActive(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused) return;

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
            SceneController.Instance.LoadSceneByName("Percussion");
        }
        if (Input.GetButtonDown("Synth"))
        {
            SceneController.Instance.LoadSceneByName("Synth");
        }
        if (Input.GetButtonDown("Replay") && !AudioRecorder.Instance.isReplaying)
        {
            AudioRecorder.Instance.PlayAudio();
        }
        if (Input.GetButtonDown("Delete"))
        {
            AudioRecorder.Instance.RemoveLastAudio();
        }
        if (Input.GetButtonDown("Record"))
        {
            AudioRecorder.Instance.Record();
            spin.SpinRecord();
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

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void EndDash()
    {
        isDashing = false;
    }


}
