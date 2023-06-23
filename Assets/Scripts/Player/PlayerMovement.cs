using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField]private bool isJumping = true;
    private Rigidbody2D rb;
    private RaycastHit2D raycastHit;


    public bool _gameStop = false;
    public GameObject _dieEffect;

    //try Again Canvas
    public GameObject tryAgainButton;

    //Ses
    public AudioSource JumpSounds;
    public AudioSource DeathSounds;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TimeController.Instance.ChangeTimeScale(0.2f);
    }

    void Update()
    {
        if (_gameStop == true)
        {
            tryAgainButton.SetActive(true);
            return;
        }
        float moveX = Input.GetAxis("Horizontal");

        // Yatay hareket
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Zýplama
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            JumpSounds.Play();
        }

        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            JumpSounds.Play();
        }

        if (_gameStop == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            tryAgainButton.SetActive(true);
           
        }

        #region time
        if (isJumping == true)
        {
            TimeController.Instance.ChangeTimeScale(1f);
        }
        else
        {
            TimeController.Instance.ChangeTimeScale(0.2f);
        }
        if (Input.GetMouseButton(0))
        {
            TimeController.Instance.ChangeTimeScale(1f);
            if (Input.GetMouseButtonUp(0))
            {
                TimeController.Instance.ChangeTimeScale(0.2f);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            TimeController.Instance.ChangeTimeScale(1f);
            if (Input.GetKeyUp(KeyCode.A)) { TimeController.Instance.ChangeTimeScale(0.2f); }
        }

        if (Input.GetKey(KeyCode.D))
        {
            TimeController.Instance.ChangeTimeScale(1f);
            if (Input.GetKeyUp(KeyCode.D)) { TimeController.Instance.ChangeTimeScale(0.2f); }
        }
        #endregion

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            DeathSounds.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            var eyes = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < eyes.Length; i++)
            {
                eyes[i].enabled = false;
            }
            Instantiate(_dieEffect, transform.position, transform.rotation);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

    }

    public void HomeAndAgainButton()
    {
        SceneManager.LoadScene(0);
        //_gameStop = false;
        //rb.constraints = RigidbodyConstraints2D.None;
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //tryAgainButton.SetActive(false);
        //homeButton.SetActive(false);
        //GetComponent<SpriteRenderer>().enabled = true;
        //var eyes = GetComponentsInChildren<SpriteRenderer>();
        //for (int i = 0; i < eyes.Length; i++)
        //{
        //    eyes[i].enabled = true;
        //}
    }
    public void Bekle(bool _w)
    {
        _gameStop = _w;
    }

}
