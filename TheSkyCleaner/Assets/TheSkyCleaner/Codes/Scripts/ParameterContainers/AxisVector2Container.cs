using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AxisVector2Container", menuName = "Scriptable Objects/Parameter Containers/AxisVector2Container")]
public class AxisVector2Container : RuntimeScriptableObject
{
    [SerializeField] private Vector2 m_initialValue;
    [SerializeField] private Vector2 m_value;

    public event UnityAction<Vector2> OnValueChanged = delegate { };

    public Vector2 Value
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
