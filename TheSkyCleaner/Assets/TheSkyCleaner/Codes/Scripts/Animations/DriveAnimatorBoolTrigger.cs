using System.Collections;
using UnityEngine;

public class DriveAnimatorBoolTrigger : MonoBehaviour
{
    [SerializeField] private string m_name;

    private Animator m_animator;
    private Coroutine m_coroutine;
    [SerializeField] private float m_resetDelay;
    [SerializeField] private bool m_isTimed;

    private WaitForSeconds m_delay;
    private WaitForEndOfFrame m_endOfFrame;


    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_delay = new(m_resetDelay);
    }

    public void Drive()
    {
        m_coroutine ??= StartCoroutine(Trigger());
    }

    private IEnumerator Trigger()
    {
        m_animator.SetBool(m_name, true);
        if (m_isTimed)
        {
            yield return m_delay;
        }
        else
        {
            yield return m_endOfFrame;
        }
        m_animator.SetBool(m_name, false);
        m_coroutine = null;
    }
}
