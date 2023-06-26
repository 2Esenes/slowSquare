using UnityEngine;

public class GranadeBulletCVontroller : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D boxCollider;

    public GameObject _explosionPrefab;
    private float explosionTime;

    //ses
    public GameObject ExplosionVoice;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        explosionTime = 3;
        ExplosionVoice = GameObject.Find("ExplosionSes");
    }

    private void Update()
    {
        explosionTime -= Time.deltaTime;
        if (explosionTime < 0)
        {
            boxCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canManipulateMaterial = false;
        if (collision.transform.tag == "Enemy")
        {
            ExplosionVoice.GetComponent<AudioSource>().Play();
            Instantiate(_explosionPrefab , transform.position, transform.rotation);
            collision.GetComponent<EnemyBasicController>().Die();
            canManipulateMaterial = true;
        }
        if (collision.transform.tag == "Player")
        {
            ExplosionVoice.GetComponent<AudioSource>().Play();
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            collision.GetComponent<PlayerMovement>().Bekle(true);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Ground")
        {
            ExplosionVoice.GetComponent<AudioSource>().Play();
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            canManipulateMaterial = true;
        }

        if (canManipulateMaterial)
        {
            ShockWaveController.Instance.SetPosition(transform.position);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            explosionTime = 0;
        }
    }



}
