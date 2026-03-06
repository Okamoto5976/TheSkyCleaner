using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntegerContainer", menuName = "Scriptable Objects/Parameter Containers/IntegerContainer")]
public class IntegerContainer : RuntimeScriptableObject
{
    [SerializeField] protected int m_initialValue;
    [SerializeField] protected int m_value;

    public int Value => m_value;
    public int InitialValue => m_initialValue;

    public virtual void SetValue(int value)
    {
        m_value = value;
    }

    protected override void OnReset() => m_value = m_initialValue;
}
