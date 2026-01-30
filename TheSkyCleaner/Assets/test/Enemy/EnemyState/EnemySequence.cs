using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySequence", menuName = "Enemy/Sequence")]
public class EnemySequence : ScriptableObject
{
    [System.Serializable]
    public struct StateMachineState
    {
        [SerializeField] private EnemyState m_state;
        [SerializeField] public float m_time;

        public readonly EnemyState State => m_state;
        public readonly float Time => m_time;
    };
    
    [SerializeField] private List<StateMachineState> m_states;

    public List<StateMachineState> States => m_states;
}