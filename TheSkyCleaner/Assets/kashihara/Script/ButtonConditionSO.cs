using UnityEngine;

// 決定の条件（条件なし、押した時、離した時）
public enum ConditionsForDecision
{
    None, 
    PushToDecide, 
    PopToDecide
}

[CreateAssetMenu(fileName = "ButtonConditionSO", menuName = "Scriptable Objects/ButtonConditionSO")]
public class ButtonConditionSO : ScriptableObject
{
    [Header("決定の条件")]
    public ConditionsForDecision ConditionsForDecision;
}
