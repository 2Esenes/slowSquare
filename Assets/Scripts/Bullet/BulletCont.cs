using DG.Tweening;
using UnityEngine;

public class BulletCont : MonoBehaviour
{
    public GameObject _hitEffect;
    public float knockBackStrength;

    //ses
    public GameObject BulletHitSounds;

    private void Start()
    {
        Destroy(gameObject, 2);
        BulletHitSounds = GameObject.Find("BulletHit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canManipulateMaterial = false;
        if (collision.transform.tag == "Enemy")
        {
            Instantiate(_hitEffect, transform.position, transform.rotation);
            BulletHitSounds.GetComponent<AudioSource>().Play();
            Vector2 knockDirection = collision.transform.position - transform.position;
            Rigidbody2D enemRb = collision.GetComponent<Rigidbody2D>();
            enemRb.AddForce(knockDirection.normalized * knockBackStrength, ForceMode2D.Impulse);
            collision.GetComponent<EnemyBasicController>().Die();
            //Destroy(gameObject);
            gameObject.SetActive(false);
            canManipulateMaterial = true;
        }
        if (collision.transform.tag == "Player")
        {
            BulletHitSounds.GetComponent<AudioSource>().Play();
            collision.GetComponent<PlayerMovement>().Bekle(true);
            Instantiate(_hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Ground")
        {
            BulletHitSounds.GetComponent<AudioSource>().Play();
            Instantiate(_hitEffect, transform.position, transform.rotation);
            //Destroy(gameObject);
            gameObject.SetActive(false);

            canManipulateMaterial = true;
        }

        if(canManipulateMaterial)
        {
            ShockWaveController.Instance.SetPosition(transform.position);
        }
    }
}
