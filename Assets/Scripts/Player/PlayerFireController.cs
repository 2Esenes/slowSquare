using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerFireController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform firePoint;
    public GameObject fireRotate;
    public float FireForce = 30;

    private Rigidbody2D rb;
    Vector2 mousePos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = mousePos - new Vector2(fireRotate.transform.position.x, fireRotate.transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        fireRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetMouseButtonDown(0)) Fire();
    }

    private void FixedUpdate()
    {
        
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * FireForce, ForceMode2D.Impulse);
    }
}
