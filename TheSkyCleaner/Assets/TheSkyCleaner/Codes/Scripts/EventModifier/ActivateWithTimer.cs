using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ActivateWithTimer : MonoBehaviour
{
    [SerializeField] private TriggerContainer m_trigger;
    [SerializeField] private UnityEvent m_events;
    [SerializeField] private float m_delay;

    private bool m_isEnabled;
    private WaitForSeconds m_waitDelay;

    private void Awake()
    {
        m_waitDelay = new(m_delay);
        m_isEnabled = true;
    }

    private void OnEnable()
    {
        m_trigger.OnValueChanged += StartTimer;
    }

    private void OnDisable()
    {
        m_trigger.OnValueChanged -= StartTimer;
        StopAllCoroutines();
    }

    public void StartTimer()
    {
        if (m_isEnabled)
        {
            m_isEnabled = false;
            StartCoroutine(InvokeTimer());
        }
    }

    IEnumerator InvokeTimer()
    {
        m_events.Invoke();
        yield return m_waitDelay;
        m_isEnabled = true;
    }
}
