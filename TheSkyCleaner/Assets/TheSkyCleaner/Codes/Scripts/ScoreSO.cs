using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ScoreSO")]
public class ScoreSO : ScriptableObject
{
    public int Score { get; private set; }

    public event Action<int> OnScoreChange;//スコア表示などに使う

    public void ResetScore()
    {
        Score = 0;
        OnScoreChange?.Invoke(Score);
    }

    public void Add(int value)
    {
        Score += value;
        OnScoreChange?.Invoke(Score);
    }
}
//[SerializeField] ScoreSO score;
//[SerializeField] Text scoreText;

//void OnEnable()
//{
//    score.OnScoreChanged += UpdateView;
//    UpdateView(score.Score);
//}

//void OnDisable()
//{
//    score.OnScoreChanged -= UpdateView;
//}

//void UpdateView(int value)
//{
//    scoreText.text = value.ToString();
//}