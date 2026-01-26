using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AxisVector2Container", menuName = "Scriptable Objects/Parameter Containers/AxisVector2Container")]
public class AxisVector2Container : ScriptableObject
{
    [SerializeField] private Vector2 m_initialValue;
    [SerializeField] private Vector2 m_value;

    public event UnityAction<Vector2> OnValueChanged = delegate { };

    public Vector2 Value => m_value;

    public void SetValue(Vector2 value)
    {
        if (m_value == value) return;
        m_value = value;
        OnValueChanged.Invoke(value);
    }
}
