using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySequence", menuName = "Enemy/Sequence")]
public class EnemySequence : ScriptableObject
{
    [System.Serializable]
    public struct StateMachineState
    {
        [SerializeField] public EnemyState state;
        [SerializeField] public Vector2 time;
    };
    
    [SerializeField] private List<StateMachineState> m_states;

    public List<StateMachineState> States => m_states;
}