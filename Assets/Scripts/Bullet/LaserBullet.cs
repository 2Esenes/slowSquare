using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LaserBullet : MonoBehaviour
{
    [SerializeField] LayerMask[] _layerMask;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] GameObject _bulletHitSound;
    [SerializeField] GameObject _hitFX;

    [SerializeField] float _duration = 0.25f;
    [SerializeField] float _delay = 0.25f;

    private void Awake()
    {
        _bulletHitSound = GameObject.Find("BulletHit");
        var startingPosition = transform.position;
        var mask = _layerMask[0];

        for(int i = 1; i < _layerMask.Length; i++)
            mask |= _layerMask[i];

        var hit = Physics2D.Raycast(transform.position, transform.up, 1000f, mask);

        transform.position = hit.point;

        if(hit.collider.tag == "Enemy")
        {
            _bulletHitSound.GetComponent<AudioSource>().Play();
            hit.collider.GetComponent<EnemyBasicController>().Die();
        }

        _lineRenderer.SetPosition(0, startingPosition);
        _lineRenderer.SetPosition(1, hit.point);

        float timer = 0f;
        DOTween.To(() => timer, (x) => timer = x, _duration, _duration)
            .SetDelay(_delay)
            .OnUpdate(() =>
            {
                var nextPos = Vector2.Lerp(startingPosition, hit.point, timer / _duration);
                _lineRenderer.SetPosition(0, nextPos);
            })
            .OnComplete(() => Destroy(gameObject));

        ShockWaveController.Instance.SetPosition(hit.point);

        Instantiate(_hitFX, transform.position, transform.rotation);
    }
}