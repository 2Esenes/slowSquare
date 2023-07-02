using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GreenPlayeFirstLvl : MonoBehaviour
{
    public LevelController _lvlContrller;

    AdManager _adManager;

    

    public void Init(AdManager adManager)
    {
        _adManager = adManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _adManager.ShowInterstatialByTime(OnAdIsClosed);
        }
    }
    public void AbiAnnamadimBuniHeHe()
    {
        _adManager.ShowInterstatialByTime(OnAdIsClosed);
    }

    private void OnAdIsClosed()
    {
        _lvlContrller._lVl = 2;
    }
}
