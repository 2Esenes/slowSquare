using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekSekBulletCont : MonoBehaviour
{
    public GameObject _explosionPrefab;

    float Timer = 6;
    //ses
    public GameObject BulletHitSounds;

    private void Start()
    {
        BulletHitSounds = GameObject.Find("BulletHit");
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            BulletHitSounds.GetComponent<AudioSource>().Play();
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            BulletHitSounds.GetComponent<AudioSource>().Play();
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            collision.GetComponent<EnemyBasicController>().Die();
            Destroy(gameObject);
        }
        
        if (collision.transform.tag == "Ground")
        {
            BulletHitSounds.GetComponent<AudioSource>().Play();
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
        }
    }
}
