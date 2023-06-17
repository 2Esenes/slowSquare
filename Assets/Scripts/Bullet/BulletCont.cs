using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCont : MonoBehaviour
{
    public GameObject _hitEffect;

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
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
