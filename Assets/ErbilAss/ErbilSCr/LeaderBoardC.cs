using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.SocialPlatforms.Impl;
using NaughtyAttributes;

public class LeaderBoardC : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> names;
    [SerializeField] List<TextMeshProUGUI> scores;

    [SerializeField] List<UserDataUI> _userDatas;
    [SerializeField] UserDataUI _playerData;

    private string publicLeaderBoardKey = "502429acc20560a32f68f286a04ef83b8b6088bf37e5571a4d984f33c432896f";


    private void Start()
    {
        GetLeaderBoard();
    }

    [Button]
    public void DeleteLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;

            for (int i = 0; i < msg.Length; i++)
            {
                Debug.Log("UsernameL: " + msg[i].Username);
                LeaderboardCreator.DeleteEntry(publicLeaderBoardKey, msg[i].Username);
            }

            GetLeaderBoard();
        }));

    }

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) => {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;

            for (int i = 0; i < loopLength; i++)
            {
                bool check = i < msg.Length;

                string userName = "=== EMPTY ===";
                string scoreStr = "??.?? Secs";

                if (check)
                {
                    userName = msg[i].Username;
                    scoreStr = msg[i].Score.ToString();
                }

                _userDatas[i].SetData(userName, scoreStr);
            }
        }));
    }

    public void SetLeaderBoardEntry(string username , int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, ((msg) => {
            //username.Substring(0, 4); 
            GetLeaderBoard();
        }));
    }


}
