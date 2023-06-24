using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController _instance;
    public static TimeController Instance => _instance;

    private float _currentTime = 1f;
    public float TimeScale => _currentTime;

    DateTime _lastestShowedTime;
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
}
