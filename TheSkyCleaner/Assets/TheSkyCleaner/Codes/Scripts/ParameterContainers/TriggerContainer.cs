using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TriggerContainer", menuName = "Scriptable Objects/Parameter Containers/TriggerContainer")]
public class TriggerContainer : ScriptableObject
{
    public event UnityAction OnValueChanged = delegate { };

    public void Trigger()
    {
        OnValueChanged.Invoke();
    }
}
