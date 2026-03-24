using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatContainer", menuName = "Scriptable Objects/Parameter Containers/FloatContainer")]
public class FloatContainer : RuntimeScriptableObject
{
    [SerializeField] protected float m_initialValue;
    [SerializeField] protected float m_value;

    public float InitialValue => m_initialValue;
    public virtual float Value => m_value;
    public virtual void SetValue(float value)
    {
        //Debug.Log($"Health changed {m_value} -> {value}\n{System.Environment.StackTrace}");
        m_value = value;
    }
    protected override void OnReset() => m_value = m_initialValue;
}
