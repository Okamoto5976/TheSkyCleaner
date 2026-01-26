using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BooleanContainer", menuName = "Scriptable Objects/Parameter Containers/BooleanContainer")]
public class BooleanContainer : ScriptableObject
{
    [SerializeField] private bool m_initialValue;
    [SerializeField] private bool m_value;

    public event UnityAction<bool> OnValueChanged = delegate { };

    public bool Value => m_value;

    public void SetValue(bool value)
    {
        if (m_value == value) return;
        m_value = value;
        OnValueChanged.Invoke(value);
    }
}
