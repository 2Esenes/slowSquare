using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController _instance;
    public static TimeController Instance => _instance;

    private float _currentTime = 1f;
    public float TimeScale => _currentTime;

    DateTime _lastestShowedTime;

    float _sessionTimer;
    bool _hasSessionFinished;

    float FinishTime => _sessionTimer;

    public DateTime LastestShowedTime {
        get
        {
            Debug.Log("Lastest Showed Time: " + _lastestShowedTime);
            return _lastestShowedTime;
        }
        set
        {
            _lastestShowedTime = value;
        } 
    }
    public bool ShowingFirstTime { get; set; }

    public void Init()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public void ChangeTimeScale(float newTimeScale)
    {
        _currentTime = newTimeScale;
        Time.timeScale = _currentTime;
    }

    public void StartSession()
    {
        _sessionTimer = 0f;
    }

    private void Update()
    {
        if (_hasSessionFinished) return;
        _sessionTimer += Time.deltaTime;
    }
}
