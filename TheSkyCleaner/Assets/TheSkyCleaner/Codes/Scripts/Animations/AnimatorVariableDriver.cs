using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorVariableDriver : MonoBehaviour
{
    private Animator m_animator;

    private Dictionary<string, bool> m_triggerStates;

    private WaitForEndOfFrame m_waitForEndOfFrame;

    private void Awake()
    {
        m_triggerStates = new Dictionary<string, bool>();
        m_animator = GetComponent<Animator>();
    }

    public void Drive(string name, float value)
    {
        m_animator.SetFloat(name, value);
    }

    public void Drive(string name, bool value)
    {
        m_animator.SetBool(name, value);
    }

    public void Drive(string name, int value)
    {
        m_animator.SetInteger(name, value);
    }

    public void TriggerBool (string name, float time = 0)
    {
        if (!m_triggerStates.ContainsKey(name) || !m_triggerStates[name])
        {
            StartCoroutine(Trigger(name, time));
        }
    }

    private IEnumerator Trigger(string name, float time = 0)
    {
        m_triggerStates[name] = true;
        m_animator.SetBool(name, true);
        if (time > 0)
        {
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield return m_waitForEndOfFrame;
        }
        m_animator.SetBool(name, false);
        m_triggerStates[name] = false;
    }
}
