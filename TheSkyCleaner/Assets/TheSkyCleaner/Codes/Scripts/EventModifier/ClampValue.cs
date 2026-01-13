using UnityEngine;
using UnityEngine.Events;

public class ClampValue : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> m_events;

    [SerializeField] private Vector2 m_range;

    public void OnEvent(float val)
    {
        val = Mathf.Clamp(val, m_range.x, m_range.y);
        m_events.Invoke(val);
    }
}
