using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    public bool isJumping = false;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private PlayerController controls;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public bool isGrounded;

    [Header("Animation Variables")]
    public bool isFacingRight = false;

    [Header("Camera Follow")]
    public CameraFollow _cameraFollow;

    private float _fallSpeedYDampingChangeThreshold;

    private void Awake()
    {
        controls = new PlayerController();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        _cameraFollow = _cameraFollow.GetComponent<CameraFollow>();

        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping Logic NEEDS FIXING
        Collider2D groundContact = Physics2D.OverlapCircle(groundCheck.position, checkRadius);
        if (groundContact != null)
        {
            isGrounded = groundContact.gameObject.CompareTag("Ground");
            if (isGrounded)
            {
                isJumping = false;
            }
        }
        else
        {
            isGrounded = false;
        }

        moveInput = controls.Move.Move.ReadValue<Vector2>();

        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        if (!isJumping && moveInput.y > 0 && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }

        // END OF JUMPING LOGIC

        // Camera damping while jumping

        //If falling pas a certain speed threshold

        if (rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        // If standing still or moving

        if (rb.velocity.y >= 0f && CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(false);

            CameraManager.instance.LerpedFromPlayerFalling = false;
        }
        else
        {
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        TurnCheck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnEnable()
    {
        controls.Move.Enable();
    }

    private void OnDisable()
    {
        controls.Move.Disable();
    }

    private void TurnCheck()
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);    
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = false;

            _cameraFollow.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = true;

            _cameraFollow.CallTurn();
        }
    }
}