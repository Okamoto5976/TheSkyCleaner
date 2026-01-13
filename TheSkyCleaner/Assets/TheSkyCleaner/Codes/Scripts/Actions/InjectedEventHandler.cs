using UnityEngine;
using UnityEngine.Events;

public class InjectedEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> m_event;

    public void InjectEvent(UnityAction<GameObject> unityAction)
    {
        m_event.AddListener(unityAction);
    }

    public void DoEvent()
    {
        m_event.Invoke(gameObject);
    }
}
