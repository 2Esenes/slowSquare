using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform firePoint;
    public GameObject fireRotate;
    public float FireForce = 30;

    public Transform target;
    Vector2 playerPos;
    public GameObject Player;
    public float shootTime = 3;

    public GameObject _dieEffect;

    LevelController _levelController;

    private void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    private void Update()
    {
        shootTime -= Time.deltaTime;
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        playerPos = Player.transform.position;
        Vector3 dir = playerPos - new Vector2(fireRotate.transform.position.x, fireRotate.transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        fireRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (shootTime <= 0)
        {
            Fire();
            shootTime = 3;

        }

    }

    public void Fire()
    {
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * FireForce, ForceMode2D.Impulse);
    }
    public void Die()
    {
        _levelController._lVl++;
        Instantiate(_dieEffect , transform.position , transform.rotation);
        Destroy(gameObject);
    }
}
