using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShockWaveController : MonoBehaviour
{
    private static ShockWaveController _instance;
    public static ShockWaveController Instance => _instance;

    //[SerializeField] Material _shockWaveMat;
    [SerializeField] SpriteRenderer _prefab;
    [SerializeField] CinemachineCameraShake _cameraShake;

    [SerializeField] float _shockWaveDuration = 0.25f;
    [SerializeField] float _centerDistanceMax = 0.3f;
    [SerializeField] float _centerDistanceStart = -0.1f;
    [SerializeField] float _strengthStart = -0.25f;
    [SerializeField] float _strengthDuration = 0.25f;
    [SerializeField] GameObject[] _objects;

    int _sortingOrder;
    public void Init()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public void SetPosition(Vector2 position, float size = 0.1f)
    {
        //var hitPosition = position;
        //var screenPos = Camera.main.WorldToScreenPoint(hitPosition);
        //var reso = new Vector2Int(922, 487);

        //var screenPosNormalized = new Vector2(screenPos.x / reso.x, screenPos.y / reso.y);
        _sortingOrder++;
        _sortingOrder = Mathf.Clamp(_sortingOrder, 0, 500);
        var sr =Instantiate(_prefab);
        var shockWaveObj = sr.gameObject;
        sr.transform.position = position;
        sr.sortingOrder = _sortingOrder;
        //_shockWaveMat.SetVector("_RingSpawnPosition", screenPosNormalized);
        var mat = sr.material;
        mat.SetFloat("_Size", size);
        PlayAnimation(shockWaveObj, mat);
    }

    private void PlayAnimation(GameObject shockWave, Material mat)
    {
        _cameraShake.Shake();
        //for (int i = 0; i < _objects.Length; i++)
        //    _objects[i].SetActive(true);

        float distanceTimer = _centerDistanceStart;
        DOTween.To(() => distanceTimer, (x) => distanceTimer = x, _centerDistanceMax, _shockWaveDuration)
            .OnUpdate(() =>
            {
                mat.SetFloat("_WaveDistanceFromCenter", distanceTimer);
            })
            .OnComplete(() =>
            {
                float strengthTimer = _strengthStart;
                DOTween.To(() => strengthTimer, (x) => strengthTimer = x, 0f, _strengthDuration)
                    .OnUpdate(() =>
                    {
                        mat.SetFloat("_ShockWaveStrength", strengthTimer);
                    })
                    .OnComplete(() =>
                    {
                        mat.SetFloat("_WaveDistanceFromCenter", _centerDistanceStart);
                        mat.SetFloat("_ShockWaveStrength", _strengthStart);

                        for (int i = 0; i < _objects.Length; i++)
                            _objects[i].SetActive(false);
                        Destroy(shockWave);
                    });
            });
    }
}
