using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController _instance;

    public static TimeController Instance => _instance;
    public void Init()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private float _currentTime = 1f;
    public float TimeScale => _currentTime;

    public void ChangeTimeScale(float newTimeScale)
    {
        _currentTime = newTimeScale;
        Time.timeScale = _currentTime;
    }
}
