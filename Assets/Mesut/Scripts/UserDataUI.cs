using TMPro;
using UnityEngine;

public sealed class UserDataUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _rankTextMesh;
    [SerializeField] TextMeshProUGUI _nameTextMesh;
    [SerializeField] TextMeshProUGUI _scoreTextMesh;

    public void SetData(string nameStr, string scoreStr, bool changeRank = false, string rankStr = "")
    {
        if(changeRank)
            _rankTextMesh.text = rankStr;
        _nameTextMesh.text = nameStr;
        _scoreTextMesh.text = scoreStr;
    }
}
