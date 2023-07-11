
using UnityEngine;

public class GreenPlayeFirstLvl : MonoBehaviour
{
    public LevelController _lvlContrller;

    System.Action _onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            // _lvlContrller.OpenSkillCards();
            _onTriggerEnter?.Invoke();
        }
    }

    public void RegisterOnTriggerEnter(System.Action action)
    {
        _onTriggerEnter += action;
    }

    public void UnRegisterOnTriggerEnter(System.Action action)
    {
        _onTriggerEnter -= action;
    }

    private void OnAdIsClosed()
    {
        _lvlContrller._lVl = 2;
    }
}
