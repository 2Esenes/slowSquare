using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CinemachineCameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] Settings _settings;

    CinemachineBasicMultiChannelPerlin _basicMultiChannelPerlin;

    bool _isShaking;

    private void Awake()
    {
        _basicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        if (_isShaking) return;
        _isShaking = true;

        float timer = 0f;
        DOTween.To(() => timer, (x) => timer = x, _settings.Duration, _settings.Duration)
            .OnUpdate(() =>
            {
                _basicMultiChannelPerlin.m_AmplitudeGain = _settings.Intensity * TimeController.Instance.TimeScale;
            })
            .OnComplete(() =>
            {
                _isShaking = false;
                _basicMultiChannelPerlin.m_AmplitudeGain = 0f;
            });
    }

    [System.Serializable]
    public sealed class Settings
    {
        [field: SerializeField] public float Intensity { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
    }
}
