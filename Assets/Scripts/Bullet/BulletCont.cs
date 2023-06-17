using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCont : MonoBehaviour
{
    public GameObject _hitEffect;
    public float knockBackStrength;

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Instantiate(_hitEffect, transform.position, transform.rotation);
            
            Vector2 knockDirection = collision.transform.position - transform.position;
            Rigidbody2D enemRb = collision.GetComponent<Rigidbody2D>();
            enemRb.AddForce(knockDirection.normalized * knockBackStrength, ForceMode2D.Impulse);

            Destroy(gameObject);
        }
        if (collision.transform.tag == "Player")
        {
            Instantiate(_hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Ground")
        {
            Instantiate(_hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
