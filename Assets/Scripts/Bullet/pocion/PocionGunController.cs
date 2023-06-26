using UnityEngine;

public class PocionGunController : MonoBehaviour
{
    public GameObject _hitEffect;

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canManipulateMaterial = false;
        if (collision.transform.tag == "Ground") 
        {
            Instantiate(_hitEffect , transform.position, transform.rotation);
            canManipulateMaterial = true;
        }
        if (collision.transform.tag == "Enemy")
        {
            Instantiate(_hitEffect, transform.position, transform.rotation);
            canManipulateMaterial = true;
        }

        if(canManipulateMaterial)
        {
            ShockWaveController.Instance.SetPosition(transform.position);
            Destroy(gameObject);
        }
    }

}
