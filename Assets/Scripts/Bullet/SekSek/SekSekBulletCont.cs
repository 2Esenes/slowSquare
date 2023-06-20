using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekSekBulletCont : MonoBehaviour
{
    public GameObject _explosionPrefab;

    float Timer = 6;

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            collision.GetComponent<EnemyBasicController>().Die();
            Destroy(gameObject);
        }
        
        if (collision.transform.tag == "Ground")
        {
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
        }
    }
}
