using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class TutorialController : MonoBehaviour
{
    [SerializeField]
    List<EnemyBasicController> _enemies;

    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] MainCompositionRoot _mcr;

    [SerializeField] TextMeshPro _instructionTextMesh;

    [SerializeField] TutorialSettings[] _tutorialSettings;

    [SerializeField] LevelController _levelController;

    [SerializeField] Button[] _cardButtons;
    [SerializeField] Button[] _leftButtons;

    List<EnemyBasicController> _activeEnemyList = new List<EnemyBasicController>();

    int _instructionCounter;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("TutorialLevelIsDone") == 1)
        {
            _playerMovement.RegisterOnDie(_mcr.OnPlayerDie);
            return;
        }

        for (int i = 0; i < _enemies.Count; i++)
            _enemies[i].RegisterOnDie(OnEnemyDie);

        _activeEnemyList.Add(_enemies[0]);

        for (int i = 0; i < _cardButtons.Length; i++)
        {
            _cardButtons[i].onClick.AddListener(OnCardSelectionIsDone);
            _leftButtons[i].onClick.AddListener(OnCardSelectionIsDone);
        }
        _playerMovement.RegisterOnDie(OnPlayerDie);
    }

    private void OnPlayerDie()
    {
        _playerMovement.ResetPlayer();
        _playerMovement.transform.position = new Vector3(1, 0);
    }

    public void OnEnemyDie(EnemyBasicController e)
    {
        _enemies.Remove(e);
        _activeEnemyList.Remove(e);
        if (_activeEnemyList.Count != 0) return;

        ChangeText();

        if (_instructionCounter == _tutorialSettings.Length - 1)
        {
            _playerMovement.UnRegisterOnDie(OnPlayerDie);
            _playerMovement.RegisterOnDie(_mcr.OnPlayerDie);

            for (int i = 0; i < _cardButtons.Length; i++)
            {
                _cardButtons[i].onClick.RemoveListener(OnCardSelectionIsDone);
                _leftButtons[i].onClick.RemoveListener(OnCardSelectionIsDone);
            }

            float finishTimer = 0;
            DOTween.To(() => finishTimer, (x) => finishTimer = x, 3f, 3f)
                .SetUpdate(true)
                .OnComplete(_levelController.FinishTutorialLevel);

            PlayerPrefs.SetInt("TutorialLevelIsDone", 1);
            return;
        }
        OpenCards();
    }

    private void OpenCards()
    {
        int randomInt = Random.Range(0, _cardButtons.Length);
        //print(randomInt);
        _cardButtons[randomInt].gameObject.SetActive(true);

        int randomInt2 = Random.Range(0, _leftButtons.Length);
        //print(randomInt2);
        _leftButtons[randomInt2].gameObject.SetActive(true);
    }

    private void OnCardSelectionIsDone()
    {
        ChangeText();
        for (int i = 0; i < 3; i++)
        {
            var canFire = _tutorialSettings[_instructionCounter].CanEnemyFire;
            _enemies[i].isTutoEnemy = !canFire;
            _enemies[i].Init();
            _enemies[i].gameObject.SetActive(true);
            _activeEnemyList.Add(_enemies[i]);
        }
    }

    private void ChangeText()
    {
        _instructionCounter++;
        var text = _tutorialSettings[_instructionCounter].Insturiction.Replace("\\n", "\n");
        _instructionTextMesh.text = text;
        _instructionTextMesh.colorGradient = _tutorialSettings[_instructionCounter].Color;
    }

    [System.Serializable]
    public sealed class TutorialSettings
    {
        [field: SerializeField] public string Insturiction { get; private set; }
        [field: SerializeField] public bool CanEnemyFire { get; private set; }
        [field: SerializeField] public VertexGradient Color { get; private set; }
    }
}