using System;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    [SerializeField] private IntegerContainer m_score;

    //public int Score { get; private set; }

    public event Action<int> OnScoreChange;//スコア表示などに使う

    public void ResetScore()
    {
        m_score.SetValue(0);
        OnScoreChange?.Invoke(m_score.Value);
    }

    public void Add(int value)
    {
        var score = Mathf.Max(m_score.Value + value, 0);

        m_score.SetValue(score);
        OnScoreChange?.Invoke(m_score.Value);
    }

    public void Sub(int value)
    {
        Add(-value);
    }

    public void Set(int value)
    {
        m_score.SetValue(value);
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