using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerDeathAnimation : MonoBehaviour
{
    [SerializeField] GameObject[] _objects;
    [SerializeField] RawImage _rawImage;
    [SerializeField] Material _pixelMaterial;

    [SerializeField] float _duration;
    [SerializeField] float _minPixel = 30f;
    [SerializeField] float _maxPixel = 200f;

    [Button]
    public void OnDeath()
    {
        for(int i = 0; i < _objects.Length; i++)
        {
            _objects[i].SetActive(true);
        }
        _rawImage.material = _pixelMaterial;

        float timer = _maxPixel;
        DOTween.To(() => timer, (x) => timer = x, _minPixel, _duration)
            .OnUpdate(() =>
            {
                _pixelMaterial.SetFloat("_Pixel", timer);
            })
            .timeScale = 1f;
    }
}
