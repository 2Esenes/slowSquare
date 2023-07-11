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
    bool _hasSessitonStarted;

    public float FinishTimeSeconds => _sessionTimer;

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
        _hasSessitonStarted = true;
        _hasSessionFinished = false;
        Debug.Log("Session Started");
    }

    public void FinishSession()
    {
        _hasSessionFinished = true;
        _hasSessitonStarted = false;
    }

    private void Update()
    {
        if (_hasSessionFinished) return;
        if (!_hasSessitonStarted) return;

        _sessionTimer += Time.deltaTime;
    }
}
