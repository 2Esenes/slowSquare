using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemyController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform firePoint;
    public GameObject fireRotate;
    public float FireForce = 30;

    public Transform target;
    Vector2 playerPos;
    public GameObject Player;
    public float shootTime = 3;
    public float baseShootTime = 3;

    public GameObject _dieEffect;

    LevelController _levelController;

    //farklý sc yazma diye bu var.ý ekliyom mal enes kendimede öle demem neyse
    public bool iHaveMachineGun = false;


    //cam Anim
    public GameObject cam;

    //ses
    public AudioSource BulletFire;
    public AudioSource DieVoice;

    //ates et flash yak
    private ParticleSystem muzzleFlasf;

    //annen
    public GreenPlayeFirstLvl Fuck;


    private void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        cam = FindObjectOfType<CinemachineVirtualCamera>().gameObject;

        //BulletFire = GameObject.Find("PlayerFire");
        //DieVoice = GameObject.Find("PlayerDeath");
    }

    private void Update()
    {
        if (muzzleFlasf == null) { muzzleFlasf = GetComponentInChildren<ParticleSystem>(); }
        shootTime -= Time.deltaTime;
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        playerPos = Player.transform.position + Player.transform.up * (0.5f + Random.Range(-0.2f, 0.2f));
        Vector3 dir = playerPos - new Vector2(fireRotate.transform.position.x, fireRotate.transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        fireRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlatED")
        {
            Die();
        }
    }

   
    public void Die()
    {
        _levelController._lVl++;
        Instantiate(_dieEffect, transform.position, transform.rotation);
        cam.GetComponent<Animator>().SetTrigger("camShake");
        DieVoice.Play();
        Destroy(gameObject);
    }
}
