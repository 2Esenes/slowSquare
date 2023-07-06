using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using NaughtyAttributes;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LeaderBoardC : MonoBehaviour
{
    [SerializeField] List<UserDataUI> _userDatas;
    [SerializeField] UserDataUI _playerData;
    [SerializeField] Settings _settings;
    [SerializeField] SubmitSettings _submitSettings;

    private string publicLeaderBoardKey = "502429acc20560a32f68f286a04ef83b8b6088bf37e5571a4d984f33c432896f";

    private void Start()
    {
        GetLeaderBoard();
    }
    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, true, ((msg) => {
            // int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            var list = msg.ToList();
            for (int i = 0; i < _userDatas.Count; i++)
            {
                string userName = "=== EMPTY ===";
                string scoreStr = "??.?? Secs";

                bool check = i < list.Count;


                if (check)
                {
                    var scoreInt = list[i].Score;
                    var scoreFloat = scoreInt / 100f;

                    userName = list[i].Username;
                    scoreStr = $"{scoreFloat:F2} Secs";
                    scoreStr = scoreStr.Replace(',', '.');

                    bool checkIsYou = list[i].Extra == LeaderboardCreator.UserID;
                    if (checkIsYou)
                    {
                        var data = list[i];
                        _playerData.SetData(userName, scoreStr, true, data.Rank.ToString());
                    }
                }

                _userDatas[i].SetData(userName, scoreStr);
            }
        }));
    }

    bool _settingFirstTime;
    public void SetLeaderBoardEntry(string username , int score)
    {
        _settingFirstTime = PlayerPrefs.GetInt("SettingsFirstTime", 0) == 0 ? true : false;
        if (!_settingFirstTime)
        {
            LeaderboardCreator.DeleteEntry(publicLeaderBoardKey, (b) =>
                LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, ((msg) =>
                {
                    //username.Substring(0, 4); 
                    GetLeaderBoard();
                })));
        }
        else
        {
            LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, ((msg) =>
            {
                //username.Substring(0, 4); 
                GetLeaderBoard();
            }));
            PlayerPrefs.SetInt("SettingsFirstTime", 1);
        }
    }

    bool _isMoving;
    bool _isOpened;

    [Button]
    public void ToggleLeaderBoardPanel()
    {
        if (!_isOpened && ! _isMoving)
        {
            _isOpened = true;
            MoveTransform(_settings.LeaderBoardTransform, _settings.YTargetPosition, _settings.OpenEase);
        }
        else if(_isOpened && !_isMoving)
        {
            _isOpened = false;
            MoveTransform(_settings.LeaderBoardTransform, _settings.YStartingPosition, _settings.CloseEase, () => _settings.LeaderBoardTransform.gameObject.SetActive(false));
        }
    }

    private void MoveTransform(Transform targetTransform, float yTransform, Ease ease, System.Action onComplete = null)
    {
        if (_isMoving) return;
        _isMoving = true;
        targetTransform.gameObject.SetActive(true);
        targetTransform.DOLocalMoveY(yTransform, _settings.MoveDuration)
            .SetUpdate(true)
            .SetEase(ease)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                _isMoving = false;
                });
    }

    string namePrefKey = "PlayerNameKey";
    string lastScorePrefKey = "lastScoreKey";

    int _score;
    int _lastScore;
    bool _closeActionCalled;
    float _finishSeconds;

    [Button]
    public void OpenSubmitPanel()
    {
        _closeActionCalled = false;
        var name = PlayerPrefs.GetString(namePrefKey);
        Debug.Log("Name: " + name);

        if (!string.IsNullOrEmpty(name))
            _submitSettings.InputField.text = name;

        float delayTimer = 0f;
        DOTween.To(() => delayTimer, (x) => delayTimer = x, _submitSettings.OpenDelayDuration, _submitSettings.OpenDelayDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {

                MoveTransform(_submitSettings.SubmitPanelTransform, _settings.YTargetPosition, _settings.OpenEase);

                _finishSeconds = TimeController.Instance.FinishTimeSeconds;

                var finishSecondsInt = Mathf.FloorToInt(TimeController.Instance.FinishTimeSeconds * 100f);
                _score = finishSecondsInt;

                if (!PlayerPrefs.HasKey(lastScorePrefKey))
                {
                    _lastScore = _score;
                }
                else
                    _lastScore = PlayerPrefs.GetInt(lastScorePrefKey);

                _submitSettings.ScoreText.text = $"{_finishSeconds:F2} Secs";
                _submitSettings.BestScoreText.text = $"{(_lastScore / 100f):F2} Secs";

            }
            );
    }

    public void CloseSubmitPanel()
    {
        if (_closeActionCalled) return;
        _closeActionCalled = true;
        _submitSettings.SubmitPanelTransform.DOScale(Vector3.zero, _settings.MoveDuration)
            .SetUpdate(true)
            .SetDelay(_submitSettings.CloseDelayDuration)
            .SetEase(_submitSettings.CloseEase)
            .OnComplete(() =>
            {
                _submitSettings.SubmitPanelTransform.gameObject.SetActive(false);
                _submitSettings.SubmitPanelTransform.localScale = Vector3.one;
                var localPositon = _submitSettings.SubmitPanelTransform.localPosition;
                localPositon.y = _submitSettings.YStartingPosition;
                _submitSettings.SubmitPanelTransform.localPosition = localPositon;
                ToggleLeaderBoardPanel();
            });
    }

    public void Submit()
    {
        if (_closeActionCalled) return;
        var name = _submitSettings.InputField.text;
        if (string.IsNullOrEmpty(name)) return;

        PlayerPrefs.SetString(namePrefKey, name);
        Debug.Log("Submit Name: " + name);

        PlayerPrefs.SetInt(lastScorePrefKey, _score);
        SetLeaderBoardEntry(name, _score);
        CloseSubmitPanel();
    }

    [System.Serializable]
    public sealed class Settings
    {
        [field: SerializeField] public RectTransform LeaderBoardTransform { get; private set; }
        [field: SerializeField] public float MoveDuration { get; private set; }
        [field: SerializeField] public float YStartingPosition { get; private set; }
        [field: SerializeField] public float YTargetPosition { get; private set; }
        [field: SerializeField] public Ease OpenEase { get; private set; }
        [field: SerializeField] public Ease CloseEase { get; private set; }
    }


    [System.Serializable]
    public sealed class SubmitSettings
    {
        [field: SerializeField] public RectTransform SubmitPanelTransform { get; private set; }
        [field: SerializeField] public TMP_InputField InputField { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI BestScoreText { get; private set; }
        [field: SerializeField] public Ease CloseEase { get; private set; }
        [field: SerializeField] public float YStartingPosition { get; private set; }
        [field: SerializeField] public float OpenDelayDuration { get; private set; }
        [field: SerializeField] public float CloseDelayDuration { get; private set; }
    }
}
