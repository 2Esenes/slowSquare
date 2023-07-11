using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class OnGameFinishedController : MonoBehaviour
{
    [SerializeField] AdManager _adManager;


    public void PlayAgain()
    {
        _adManager.ShowInterstatialByTime(OnAdClosed);
    }

    private void OnAdClosed()
    {
        SceneManager.LoadScene(0);
    }
}
