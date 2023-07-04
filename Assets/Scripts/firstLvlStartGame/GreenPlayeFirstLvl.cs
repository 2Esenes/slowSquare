using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GreenPlayeFirstLvl : MonoBehaviour
{
    public LevelController _lvlContrller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _lvlContrller.OpenSkillCards();
        }
    }

    private void OnAdIsClosed()
    {
        _lvlContrller._lVl = 2;
    }
}
