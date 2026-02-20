using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntegerContainer", menuName = "Scriptable Objects/Parameter Containers/IntegerContainer")]
public class IntegerContainer : RuntimeScriptableObject
{
    [SerializeField] private int m_initialValue;
    [SerializeField] private int m_value;

    public int Value => m_value;

    public void SetValue(int value)
    {
        m_value = value;
    }

    protected override void OnReset() => m_value = m_initialValue;
}
