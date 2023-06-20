using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PocionBullet : MonoBehaviour
{
    public float timeMan;
    public BoxCollider2D collider;


    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        Destroy(gameObject, 4);
    }

    private void Update()
    {
        Invoke("DisableMyCollider", timeMan);
    }


    private void DisableMyCollider()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().Bekle(true);
        }
        if (collision.transform.tag == "Enemy")
        {
            collision.GetComponent<EnemyBasicController>().Die();
        }
    }

}
