using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFireController : MonoBehaviour
{
    public GameObject BulletPrefab;
    //public GameObject GranadePrefab;
    public Transform firePoint;
    public GameObject fireRotate;
    public GameObject ChosingGun;
    public bool _machineGun = false;
    public float FireForce = 30;  //granade i�in 10 ,gun i�in 30 , machine gun i�inde 30  
    
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

        if (_machineGun == false)
        {
            if (Input.GetMouseButtonDown(0) && fireTimer >= 1) { Fire(ChosingGun); fireTimer = 0; }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && fireTimer >= 1)
            {
                Fire(ChosingGun);
                Invoke("MachineFire", 0.1f);
                Invoke("MachineFire", 0.2f);
                fireTimer = 0;
            }
        }
        print(_machineGun);

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

    private void MachineFire()
    {
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * FireForce, ForceMode2D.Impulse);
    }

    public void MYGun(GameObject _gun)
    {
        ChosingGun = _gun;
    }
    public void MYFireForce(float _force)
    {
        FireForce = _force;
    }
    public void IsMachineGun(bool _machine)
    {
        _machineGun = _machine;
    }

}
