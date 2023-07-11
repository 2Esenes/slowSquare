using Cinemachine;
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
    [SerializeField] Vector2 _minMaxShootTime = new Vector2(1.5f, 3f);
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

    public bool isTutoEnemy;


    System.Action<EnemyBasicController> _onDie;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _levelController = FindObjectOfType<LevelController>();
        cam = FindObjectOfType<CinemachineVirtualCamera>().gameObject;

        //BulletFire = GameObject.Find("PlayerFire");
        //DieVoice = GameObject.Find("PlayerDeath");
        shootTime = GetShootTime();
    }

    float GetShootTime()
    {
        return Random.Range(_minMaxShootTime.x, _minMaxShootTime.y);
    }

    private void Update()
    {
        if (isTutoEnemy == true) return;

        if (muzzleFlasf == null) { muzzleFlasf = GetComponentInChildren<ParticleSystem>(); }
        shootTime -= Time.deltaTime;
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        playerPos = Player.transform.position + Player.transform.up * (0.5f + Random.Range(-0.2f, 0.2f));
        Vector3 dir = playerPos - new Vector2(fireRotate.transform.position.x, fireRotate.transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        fireRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        

        if (iHaveMachineGun == false)
        {
            if (shootTime <= 0)
            {
                Fire();
                shootTime = GetShootTime();
            }
        }
        else
        {
            //burayada boolu true yap machine gun olan þeyleri yaz
            if (shootTime <= 0)
            {
                
                Fire();
                Invoke("Fire", 0.2f);
                Invoke("Fire", 0.4f);
                
                shootTime = GetShootTime();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlatED") 
        {
            Die();
        }
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * FireForce, ForceMode2D.Impulse);
        BulletFire.Play();
        muzzleFlasf.Play();
    }
    public void Die()
    {  
        _levelController._lVl++;
        Instantiate(_dieEffect , transform.position , transform.rotation);
        cam.GetComponent<Animator>().SetTrigger("camShake");
        DieVoice.Play();
        _onDie?.Invoke(this);
        Destroy(gameObject);
    }

    public void RegisterOnDie(System.Action<EnemyBasicController> action)
    {
        _onDie += action;
    }

    public void UnRegisterOnDie(System.Action<EnemyBasicController> action)
    {
        _onDie -= action;
    }
}
