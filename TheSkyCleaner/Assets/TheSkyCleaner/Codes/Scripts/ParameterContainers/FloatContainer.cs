using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatContainer", menuName = "Scriptable Objects/Parameter Containers/FloatContainer")]
public class FloatContainer : ScriptableObject
{
    [SerializeField] private float m_initialValue;
    [SerializeField] private float m_value;

    public event UnityAction<float> OnValueChanged = delegate { };

    public float Value
    {
        get => m_value;
        set
        {
            if (m_value == value) return;
            m_value = value;
            OnValueChanged.Invoke(value);
        }
    }
}
