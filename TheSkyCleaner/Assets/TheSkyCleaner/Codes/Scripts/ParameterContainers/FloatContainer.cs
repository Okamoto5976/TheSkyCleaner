using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatContainer", menuName = "Scriptable Objects/Parameter Containers/FloatContainer")]
public class FloatContainer : ScriptableObject
{
    [SerializeField] private float m_initialValue;
    [SerializeField] private float m_value;

    public float Value
    {
        get => m_value;
        set => m_value = value;
    }
}
