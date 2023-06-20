using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GranadeBulletCVontroller : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D boxCollider;

    public GameObject _explosionPrefab;
    private float explosionTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        explosionTime = 3;
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
        if (collision.transform.tag == "Enemy")
        {
            Instantiate(_explosionPrefab , transform.position, transform.rotation);
            collision.GetComponent<EnemyBasicController>().Die();
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Player")
        {
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            collision.GetComponent<PlayerMovement>().Bekle(true);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Ground")
        {
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
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