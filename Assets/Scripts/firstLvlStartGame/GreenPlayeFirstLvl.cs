using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GreenPlayeFirstLvl : MonoBehaviour
{
    public LevelController _lvlContrller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _lvlContrller._lVl = 2;
            
        }
    }
}
