using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyAngryAnimation : MonoBehaviour
{
    [SerializeField] Transform _leftEye;
    [SerializeField] Transform _rightEye;
    [SerializeField] Settings _settings;

    [SerializeField] float _testDuration;

    [Button]
    public void Test()
    {
        BecomeAngry(_testDuration);
    }

    public void BecomeAngry(float duration)
    {
        _leftEye.DOScale(_settings.Scale, duration);
        _leftEye.DOLocalRotate(Vector3.forward * -_settings.ZRotation, duration);
        _leftEye.DOLocalMoveY(_settings.YPosition, duration);

        _rightEye.DOScale(_settings.Scale, duration);
        _rightEye.DOLocalRotate(Vector3.forward * _settings.ZRotation, duration);
        _rightEye.DOLocalMoveY(_settings.YPosition, duration);
    }

    [System.Serializable]
    public sealed class Settings
    {
        [field: SerializeField] public float ZRotation { get; private set; }
        [field: SerializeField] public Vector3 Scale { get; private set; }
        [field: SerializeField] public float YPosition { get; private set; }
    }
}
