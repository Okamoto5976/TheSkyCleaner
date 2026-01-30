using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StringContainer", menuName = "Scriptable Objects/Parameter Containers/StringContainer")]
public class StringContainer : RuntimeScriptableObject
{
    [SerializeField] private string m_initialValue;
    [SerializeField] private string m_value;

    public string Value => m_value;

    public void SetValue(string value)
    {
        m_value = value;
    }

    protected override void OnReset() => m_value = m_initialValue;
}
