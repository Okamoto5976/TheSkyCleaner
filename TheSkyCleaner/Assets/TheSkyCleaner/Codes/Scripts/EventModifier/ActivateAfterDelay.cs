using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ActivateAfterDelay : MonoBehaviour
{
    [SerializeField] private float m_delay;
    [SerializeField] private UnityEvent m_events;

    private WaitForSeconds m_delayTime;
    private Coroutine m_Coroutine;
    private void Awake()
    {
        m_delayTime = new(m_delay);
        m_Coroutine = null;
    }

    public void DoEvent()
    {
        m_Coroutine ??= StartCoroutine(OnDelay());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        m_Coroutine = null;
    }

    private IEnumerator OnDelay()
    {
        yield return m_delayTime;
        m_events.Invoke();
        m_Coroutine = null;
    }
}
