using UnityEngine;
using UnityEngine.Events;

public class ActivateOnEnable : MonoBehaviour
{
    [SerializeField] private UnityEvent m_events;

    private void OnEnable()
    {
        m_events.Invoke();
    }
}
