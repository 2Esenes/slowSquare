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

    public GameObject[] skillCards;
    public GameObject[] skillCardsLeft;

    GameObject player;
    private int _nextLvl = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    private void Update()
    {
        
        ScoreText.text = _lVl.ToString();
        if(_lVl == 2 && _nextLvl == 0) { _nextLvl = 2; }
        if (_lVl == 2 && _nextLvl == 2) 
        {
            _nextLvl = 3;

            int randomInt = Random.Range(0, skillCards.Length);
            //print(randomInt);
            skillCards[randomInt].SetActive(true);
            
            int randomInt2 = Random.Range(0, skillCardsLeft.Length);
            //print(randomInt2);
            skillCardsLeft[randomInt2].SetActive(true); 
        }
        if (_lVl == 4 && _nextLvl == 3) 
        {
            _nextLvl = 4;

        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            int randomInt = Random.Range(0, skillCards.Length);
            for (int i = 0; i < skillCards.Length; i++)
            {
                skillCards[i].SetActive(false);
            }
            skillCards[randomInt].SetActive(true);
        }

    }
    public void PlayerTransformLvl(float x , float y)
    {
        //silah seçme butonunda bu kýsmý da çaðýr
        player.transform.position = new Vector2(x, y);
    }

    //her skill kartýnda çaðýr
    public void StartNextLvl()
    {
        ScoreText.text = _lVl.ToString();
        if (_lVl == 2)
        {
            Lvl2.SetActive(true);
            PlayerTransformLvl(1, 0);
        }
        if (_lVl == 4) { Lvl3.SetActive(true); }

    }
    //her Skill Kartýnda çaðýr
    public void CloseSkillCard()
    {
        for (int i = 0; i < skillCardsLeft.Length; i++)
        {
            skillCardsLeft[i].SetActive(false);
        }

        for (int i = 0; i < skillCards.Length; i++)
        {
            skillCards[i].SetActive(false);
        }
    }

}
