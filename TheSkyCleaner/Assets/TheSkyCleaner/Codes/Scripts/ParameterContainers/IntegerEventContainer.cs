using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntegerEventContainer", menuName = "Scriptable Objects/Parameter Containers/Integer Event Container")]
public class IntegerEventContainer : IntegerContainer
{
    public event UnityAction<int> OnValueChanged = delegate { };

    public override void SetValue(int value)
    {
        if (m_value == value) return;
        m_value = value;
        OnValueChanged.Invoke(value);
    }
}
