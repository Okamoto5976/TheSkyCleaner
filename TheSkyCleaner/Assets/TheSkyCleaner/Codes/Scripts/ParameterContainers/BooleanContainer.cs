using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BooleanContainer", menuName = "Scriptable Objects/Parameter Containers/BooleanContainer")]
public class BooleanContainer : ScriptableObject
{
    [SerializeField] private bool m_initialValue;
    [SerializeField] private bool m_value;

    public event UnityAction<bool> OnValueChanged = delegate { };

    public bool Value
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
