using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TriggerContainer", menuName = "Scriptable Objects/Parameter Containers/TriggerContainer")]
public class TriggerContainer : ScriptableObject
{
    public event UnityAction OnTrigger = delegate { };
    public void Trigger() => OnTrigger.Invoke();
}
