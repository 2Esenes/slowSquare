using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlatController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.GetComponent<EnemyBasicController>().Die();
        }
        if (collision.transform.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().Bekle(true);
        }
    }
}
