using UnityEngine;
using UnityEngine.Events;

public class SmoothedLerp : MonoBehaviour
{
    [SerializeField] private float m_lerpTime;
    [SerializeField] private UnityEvent<float> m_event;

    private float m_value;

    private void Awake()
    {
        m_value = 0;
    }

    public void Lerp(float dir)
    {
        m_value = Mathf.Lerp(m_value, dir, Time.deltaTime * m_lerpTime);
        m_event.Invoke(m_value);
    }
}
