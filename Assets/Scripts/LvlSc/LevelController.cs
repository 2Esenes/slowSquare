using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public int _lVl; //died enemy count
    public Text ScoreText;
    #region LvL Platforms

    public GameObject Lvl1;
    public GameObject Lvl2;
    public GameObject Lvl3;
    public GameObject Lvl4;
    public GameObject Lvl5;
    public GameObject Lvl6;
    public GameObject Lvl7;
    public GameObject Lvl8;
    public GameObject Lvl9;
    public GameObject Lvl10;
    public GameObject Lvl11;
    public GameObject Lvl12;
    public GameObject Lvl13;
    public GameObject Lvl14;
    public GameObject Lvl15;
    #endregion

    GameObject player;
    private bool _nextLvl;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    private void Update()
    {
        ScoreText.text = _lVl.ToString();
        if (_lVl == 2) { Lvl2.SetActive(true); }
        if (_lVl == 4) { Lvl3.SetActive(true); }

    }
    public void PlayerTransformLvl(float x , float y)
    {
        //silah seçme butonunda bu kýsmý da çaðýr
        player.transform.position = new Vector2(x, y);
    }



}
