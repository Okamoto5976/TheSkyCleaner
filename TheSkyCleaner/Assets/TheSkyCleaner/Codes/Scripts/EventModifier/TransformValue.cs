using UnityEngine;
using UnityEngine.Events;

public class TransformValue : MonoBehaviour
{
    [SerializeField] private Vector3 m_transformingValues;
    [SerializeField] private UnityEvent<float> m_events;

    public void Transform(float val)
    {
        float newVal;
        if (val > 0)
        {
            newVal = (float)(val * (m_transformingValues.z - m_transformingValues.y) + m_transformingValues.y);
        }
        else if (val < 0)
        {
            newVal = (float)(val * (m_transformingValues.y - m_transformingValues.x) + m_transformingValues.y);
        }
        else
        {
            newVal = m_transformingValues.y;
        }
        m_events.Invoke(newVal);
    }
}
