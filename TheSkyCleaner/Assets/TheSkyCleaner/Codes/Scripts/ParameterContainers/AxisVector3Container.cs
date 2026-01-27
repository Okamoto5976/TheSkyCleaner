using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AxisVector3Container", menuName = "Scriptable Objects/Parameter Containers/AxisVector3Container", order = 1)]
public class AxisVector3Container : RuntimeScriptableObject
{
    [SerializeField] private Vector3 m_initialValue;
    [SerializeField] private Vector3 m_value;

    public event UnityAction<Vector3> OnValueChanged = delegate { };

    public Vector3 Value => m_value;

    public void SetValue(Vector3 value)
    {
        if (m_value == value) return;
        m_value = value;
        OnValueChanged.Invoke(value);
    }

    protected override void OnReset() => m_value = m_initialValue;
}
