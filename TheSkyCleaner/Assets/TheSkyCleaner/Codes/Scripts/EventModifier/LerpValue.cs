using UnityEngine;
using UnityEngine.Events;

public class LerpValue : MonoBehaviour
{
    [SerializeField] private Vector2 m_range;
    [SerializeField] private float m_lerpDuration;
    [SerializeField] private UnityEvent m_eventStart;
    [SerializeField] private UnityEvent<float> m_eventHoldDuration;
    [SerializeField] private UnityEvent m_eventHoldSucceeded;
    [SerializeField] private UnityEvent m_eventEndSuccess;
    [SerializeField] private UnityEvent m_eventEndCancelled;
    [SerializeField] private UnityEvent m_eventEnd;

    private int m_state;
    private float m_lerpTime;

    private void Awake()
    {
        m_state = 0;
    }

    private void Update()
    {
        float val;
        if (m_state == 0)
        {
            return;
        }
        m_lerpTime = Mathf.Clamp01(m_lerpTime + Time.deltaTime * m_state);
        val = Mathf.Lerp(m_range.x, m_range.y, m_lerpTime);
        if (m_lerpTime == 1)
        {
            m_eventHoldSucceeded.Invoke();
            m_state = 0;
        }
        m_eventHoldDuration.Invoke(val);
    }

    public void StartLerp()
    {
        m_eventStart.Invoke();
        m_state = 1;
    }

    public void StopLerp()
    {
        m_state = 0;
    }

    public void ReleaseLerp()
    {
        if (m_lerpTime == 1)
        {
            m_eventEndSuccess.Invoke();
            ResetLerp();
        }
        else
        {
            m_eventEndCancelled.Invoke();
            m_state = -1;
        }
    }

    public void ResetLerp()
    {
        m_state = 0;
        m_lerpTime = 0;
        m_eventEnd.Invoke();
    }
}
