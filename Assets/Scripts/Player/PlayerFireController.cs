using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFireController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject GranadePrefab;
    public Transform firePoint;
    public GameObject fireRotate;
    public GameObject ChosingGun;

    public float FireForce = 30;  //granade için 10 ,gun için 30 , machine gun içinde 30  
    
    Vector2 mousePos;

    //FireTime
    public float fireTimer = 1;

    public GameObject _reloadSlider;

    private void Start()
    {
        ChosingGun = BulletPrefab;
    }
    private void Update()
    {
        fireTimer += Time.deltaTime;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = mousePos - new Vector2(fireRotate.transform.position.x, fireRotate.transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        fireRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetMouseButtonDown(0) && fireTimer >= 1) { Fire(ChosingGun); fireTimer = 0; }

        _reloadSlider.GetComponent<Slider>().value = fireTimer;
        if (fireTimer > 1)
        {
            _reloadSlider.SetActive(false);
        }
        if (fireTimer <= 1)
        {
            _reloadSlider.SetActive(true);
        }
    
    }

    public void Fire(GameObject gun )
    {
        GameObject bullet = Instantiate(gun, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * FireForce, ForceMode2D.Impulse);
    }


    public void MyGun(GameObject guns, float bulletForce)
    {
        ChosingGun = guns;
        FireForce = bulletForce;
    }

    public void NedenOlmuyon(GameObject _gun)
    {
        ChosingGun = _gun;
    }


}
