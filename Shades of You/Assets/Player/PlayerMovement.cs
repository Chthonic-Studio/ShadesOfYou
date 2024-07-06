using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    private bool isJumping = false;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private PlayerController controls;

    private void Awake()
    {
        controls = new PlayerController();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = controls.Move.Move.ReadValue<Vector2>();

        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        if (!isJumping && moveInput.y > 0)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }

        if (isJumping && moveInput.y == 0)
        {
            isJumping = false;
        }
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
}