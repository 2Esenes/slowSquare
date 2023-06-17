using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField]private bool isJumping = true;
    private Rigidbody2D rb;
    private RaycastHit2D raycastHit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 0.2f;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        // Yatay hareket
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Zýplama
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }

        //zaman

        if (isJumping == true)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.2f;
        }
        if (Input.GetMouseButton(0))
        {
            Time.timeScale = 1f;
            if (Input.GetMouseButtonUp(0))
            {
                Time.timeScale = 0.2f;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            Time.timeScale = 1f;
            if (Input.GetKeyUp(KeyCode.A)) { Time.timeScale = 0.2f; }
        }

        if (Input.GetKey(KeyCode.D))
        {
            Time.timeScale = 1f;
            if (Input.GetKeyUp(KeyCode.D)) { Time.timeScale = 0.2f; }
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
