using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEventContainer", menuName = "Scriptable Objects/Parameter Containers/Integer Event Container")]
public class FloatEventContainer : FloatContainer
{
    public event UnityAction<float> OnValueChanged = delegate { };

    public override void SetValue(float value)
    {
        if (m_value == value) return;
        m_value = value;
        OnValueChanged.Invoke(value);
    }
}
