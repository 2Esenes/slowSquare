using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public int _lVl; //died enemy count
    public Text ScoreText;
    [SerializeField] TextMeshProUGUI _scoreText;

    [SerializeField] GameObject[] _levels;
    [SerializeField] GameObject _tutorialLevel;
    #region LvL Platforms

    //public GameObject Lvl1;
    //public GameObject Lvl2;
    //public GameObject Lvl3;
    //public GameObject Lvl4;
    //public GameObject Lvl5;
    //public GameObject Lvl6;
    //public GameObject Lvl7;
    //public GameObject Lvl8;
    //public GameObject Lvl9;
    //public GameObject Lvl10;
    //public GameObject Lvl11;
    //public GameObject Lvl12;
    //public GameObject Lvl13;
    //public GameObject Lvl14;
    //public GameObject Lvl15;
    #endregion

    [SerializeField] Button[] _skillCards;

    public GameObject[] skillCards;
    public GameObject[] skillCardsLeft;

    [SerializeField]
    GreenPlayeFirstLvl[] _starts;

    [SerializeField] BackgroundColorChanger _colorChanger;
    [SerializeField] LeaderBoardC _leaderBoardManager;

    [SerializeField]
    Button[] _leaderBoardClosePanels;
    [SerializeField] Transform _playAgainButton;

    GameObject player;
    public int _nextLvl = 0;

    string _tutorialLevelPrefKey = "TutorialLevelIsDone";
    bool _tutorialLevelIsDone;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        _tutorialLevelIsDone = PlayerPrefs.GetInt(_tutorialLevelPrefKey, 0) == 1 ? true : false;

        Debug.Log("Tutorial is done: " + _tutorialLevelIsDone);

        for (int i = 0; i < _starts.Length; i++)
        {
            System.Action action = null;
            if (_tutorialLevelIsDone)
                action = OpenSkillCards;
            else
                action = StartLevel;

            _starts[i].RegisterOnTriggerEnter(action);
        }

        if(_tutorialLevelIsDone)
        {
            for (int i = 0; i < _skillCards.Length; i++)
                _skillCards[i].onClick.AddListener(StartLevel);
        }
    }

    [Button]
    public void Test()
    {
        var min = 0f;
        var seconds = 57.2550f;
        _scoreText.text = $"{min} Mins - {seconds:F2} Secs";
    }

    private void OnGameFinishLeaderBoardCloseButtons()
    {
        for(int i = 0; i < _leaderBoardClosePanels.Length; i++)
        {
            Debug.Log("Adding");
            var button = _leaderBoardClosePanels[i];
            button.onClick.AddListener(OpenPlayAgainButton);
        }
    }

    private void OpenPlayAgainButton()
    {
        Debug.Log("Working");
        _playAgainButton.gameObject.SetActive(true);
        _playAgainButton.DOScale(Vector3.one, 1f)
            .SetDelay(1f)
            .SetUpdate(true);
    }

    bool _isCardsSelectionOpened;

    public void OpenSkillCards()
    {
        bool isGameFinished = _nextLvl == _levels.Length - 2;
        _nextLvl++;
        if (isGameFinished)
        {
            StartLevel();
            TimeController.Instance.FinishSession();
            _leaderBoardManager.OpenSubmitPanel();
            OnGameFinishLeaderBoardCloseButtons();
            //var finishSeconds = TimeController.Instance.FinishTimeSeconds;
            //var min = Mathf.FloorToInt(finishSeconds / 60f);

            //var finishSecondsInt = Mathf.FloorToInt(TimeController.Instance.FinishTimeSeconds * 100f);

            return;
        }

        if (_isCardsSelectionOpened) return;

        _isCardsSelectionOpened = true;

        int randomInt = Random.Range(0, skillCards.Length);
        //print(randomInt);
        skillCards[randomInt].SetActive(true);

        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
        //print(randomInt2);
        skillCardsLeft[randomInt2].SetActive(true);
    }

    public void FinishTutorialLevel()
    {
        _tutorialLevel.SetActive(false);
        _levels[0].SetActive(true);
        _nextLvl = 0;
        PlayerTransformLvl(1, 0);
        _colorChanger.SetStartColor();

        for (int i = 0; i < _starts.Length; i++)
        {
            _starts[i].UnRegisterOnTriggerEnter(StartLevel);
            _starts[i].RegisterOnTriggerEnter(OpenSkillCards);
        }

        for (int i = 0; i < _skillCards.Length; i++)
            _skillCards[i].onClick.AddListener(StartLevel);

        _tutorialLevelIsDone = true;
    }

    public void StartLevel()
    {
        if (!_tutorialLevelIsDone)
        {
            _tutorialLevel.SetActive(true);
            _levels[0].SetActive(false);

            PlayerTransformLvl(1, 0);
            Debug.Log("Returning");
            return;
        }

        if (_nextLvl - 1 == 0)
            TimeController.Instance.StartSession();

        Debug.Log("NextLevel: " + _nextLvl);
        _levels[_nextLvl - 1].SetActive(false);
        _levels[_nextLvl].SetActive(true);
        PlayerTransformLvl(1, 0);

        _isCardsSelectionOpened = false;
        _colorChanger.ChangeBGColor();
    }

    //private void Update()
    //{
    //    ScoreText.text = _lVl.ToString();
    //    if(_lVl == 2 && _nextLvl == 0) { _nextLvl = 2; }
    //    if (_lVl == 2 && _nextLvl == 2) // 1 den 2 ye
    //    {
    //        _nextLvl = 3;

    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true); 
    //    }
    //    if (_lVl == 3 && _nextLvl == 3) // 2 den 3 e
    //    {
    //        _nextLvl = 4;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 5 && _nextLvl == 4) 
    //    {
    //        _nextLvl = 5;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 9 && _nextLvl == 5) 
    //    {
    //        _nextLvl = 6;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 11 && _nextLvl == 6)
    //    {
    //        _nextLvl = 7;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 13 && _nextLvl == 7)
    //    {
    //        _nextLvl = 8;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 16 && _nextLvl == 8)
    //    {
    //        _nextLvl = 9;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 18 && _nextLvl == 9)
    //    {
    //        _nextLvl = 10;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 20 && _nextLvl == 10)
    //    {
    //        _nextLvl = 11;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 24 && _nextLvl == 11)
    //    {
    //        _nextLvl = 12;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 27 && _nextLvl == 12)
    //    {
    //        _nextLvl = 13;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 33 && _nextLvl == 13)
    //    {
    //        _nextLvl = 14;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 39 && _nextLvl == 14)
    //    {
    //        _nextLvl = 15;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //    if (_lVl == 45 && _nextLvl == 15)
    //    {
    //        _nextLvl = 16;
    //        int randomInt = Random.Range(0, skillCards.Length);
    //        //print(randomInt);
    //        skillCards[randomInt].SetActive(true);

    //        int randomInt2 = Random.Range(0, skillCardsLeft.Length);
    //        //print(randomInt2);
    //        skillCardsLeft[randomInt2].SetActive(true);
    //    }
    //}

    public void PlayerTransformLvl(float x, float y)
    {
        //silah seçme butonunda bu kýsmý da çaðýr
        player.transform.position = new Vector2(x, y);
    }

    //her skill kartýnda çaðýr
    //public void StartNextLvl()
    //{
    //    ScoreText.text = _lVl.ToString();
    //    if (_lVl == 2) // 1 den 2 ye
    //    {
    //        Lvl2.SetActive(true);
    //        Lvl1.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 3) // 2 den 3 e
    //    {
    //        Lvl3.SetActive(true);
    //        Lvl2.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 5) // 2 den 3 e
    //    {
    //        Lvl4.SetActive(true);
    //        Lvl3.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 9) // 2 den 3 e
    //    {
    //        Lvl5.SetActive(true);
    //        Lvl4.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 11) // 2 den 3 e
    //    {
    //        Lvl6.SetActive(true);
    //        Lvl5.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 13) // 2 den 3 e
    //    {
    //        Lvl7.SetActive(true);
    //        Lvl6.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 16) // 2 den 3 e
    //    {
    //        Lvl8.SetActive(true);
    //        Lvl7.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 18) // 2 den 3 e
    //    {
    //        Lvl9.SetActive(true);
    //        Lvl8.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 20) // 2 den 3 e
    //    {
    //        Lvl10.SetActive(true);
    //        Lvl9.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 24) // 2 den 3 e
    //    {
    //        Lvl11.SetActive(true);
    //        Lvl10.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 27) // 2 den 3 e
    //    {
    //        Lvl12.SetActive(true);
    //        Lvl11.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 33) // 2 den 3 e
    //    {
    //        Lvl13.SetActive(true);
    //        Lvl12.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 39) // 2 den 3 e
    //    {
    //        Lvl14.SetActive(true);
    //        Lvl13.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }
    //    if (_lVl == 45) // 2 den 3 
    //    {
    //        Lvl15.SetActive(true);
    //        Lvl14.SetActive(false);
    //        PlayerTransformLvl(1, 0);
    //    }

    //    _colorChanger.ChangeBGColor();

    //}
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
