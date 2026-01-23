using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatContainer", menuName = "Scriptable Objects/Parameter Containers/FloatContainer")]
public class FloatContainer : RuntimeScriptableObject
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

    protected override void OnReset()
    {
        OnValueChanged.Invoke(m_value = m_initialValue);
    }
}
