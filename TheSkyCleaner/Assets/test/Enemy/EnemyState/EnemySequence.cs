using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySequence", menuName = "Enemy/Sequence")]
public class EnemySequence : ScriptableObject
{
    public enum EnemyVisualType
    {
        Type1,
        Type2,
        Type3
    }

    [System.Serializable]
    public struct StateMachineState
    {
        [SerializeField] public EnemyState state;
        [SerializeField] public Vector2 time;
    };
    
    [SerializeField] private List<StateMachineState> m_states;

    [SerializeField] private EnemyVisualType m_visualtype;

    public List<StateMachineState> States => m_states;
    public EnemyVisualType VisualType => m_visualtype;
}