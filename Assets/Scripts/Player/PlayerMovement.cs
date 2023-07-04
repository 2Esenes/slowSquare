using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField]private bool isJumping = true;
    private Rigidbody2D rb;
    private RaycastHit2D raycastHit;

    [SerializeField] PlayerDeathAnimation _playerDeathAnimation;
    [SerializeField] PunchSettings _jumpPunchSettings;
    [SerializeField] PunchSettings _landingPunchSettings;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] ParticleSystem _landingParticle;

    public bool _gameStop = false;
    public GameObject _dieEffect;

    //try Again Canvas
    public GameObject tryAgainButton;

    //Ses
    public AudioSource JumpSounds;
    public AudioSource DeathSounds;

    System.Action _onDie;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TimeController.Instance.ChangeTimeScale(0.2f);
    }

    Tween _jumpPunchTween;
    Tween _landingPunchTween;

    void Update()
    {
        if (_isDeath && Input.anyKeyDown)
        {
            HomeAndAgainButton();
        }

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


            if (_landingPunchTween!= null)
            {
                _landingPunchTween.Kill();
                _landingPunchTween = null;
            }

            transform.localScale = Vector3.one;
            _jumpPunchTween = transform.DOPunchScale(_jumpPunchSettings.Punch, _jumpPunchSettings.Duration, _jumpPunchSettings.Vibrato, _jumpPunchSettings.Elasticity)
                .SetUpdate(true);
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

    bool _onAir;
    [SerializeField] float _rayDistance;
    private void FixedUpdate()
    {
        var raycastHit = Physics2D.Raycast(transform.position, transform.up * -1, _rayDistance, _groundMask);
        if(raycastHit.collider == null)
        {
            _onAir = true;
        }
        else if(raycastHit.collider != null && _onAir && raycastHit.collider.CompareTag("Ground"))
        {
            if(_onAir)
            {
                if(_jumpPunchTween != null)
                {
                    _jumpPunchTween.Kill();
                    _jumpPunchTween = null;
                }
                
                transform.localScale = Vector3.one;
                _landingPunchTween = transform.DOPunchScale(_landingPunchSettings.Punch, _landingPunchSettings.Duration, _landingPunchSettings.Vibrato, _landingPunchSettings.Elasticity)
                    .SetUpdate(true);

                _landingParticle.Play();
            }
            _onAir = false;
        }
    }

    bool _isDeath;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDeath) return;

        if (collision.transform.tag == "Bullet")
        {
            Debug.Log("collision Name: " + collision.name);
            DeathSounds.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            var eyes = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < eyes.Length; i++)
            {
                eyes[i].enabled = false;
            }
            Instantiate(_dieEffect, transform.position, transform.rotation);
            _isDeath = true;
            _onDie?.Invoke();
        }
    }

    public void RegisterOnDie(System.Action action)
    {
        _onDie += action;
    }

    public void UnRegisterOnDie(System.Action action)
    {
        _onDie -= action;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    public void HomeAndAgainButton([CallerMemberName] string callerName = "")
    {
        Debug.Log("callerName: " + callerName);
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

    [System.Serializable]
    public sealed class PunchSettings
    {
        [field: SerializeField] public Vector3 Punch { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public int Vibrato { get; private set; } = 10;
        [field: SerializeField] public float Elasticity { get; private set; } = 1f;
    }
}
