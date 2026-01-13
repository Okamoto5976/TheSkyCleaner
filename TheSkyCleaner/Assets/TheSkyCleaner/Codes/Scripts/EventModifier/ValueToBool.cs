using UnityEngine;
using UnityEngine.Events;

public class ValueToBool : MonoBehaviour
{
    [SerializeField] private bool m_isNegative;
    [SerializeField] private bool m_isNeutral;
    [SerializeField] private bool m_isPositive;
    [SerializeField] private UnityEvent m_eventTrue;
    [SerializeField] private UnityEvent m_eventFalse;
    
    private bool m_state;

    private void Awake()
    {
        m_state = false;
    }

    public void Transform(float val)
    {
        bool res = val > 0 ? m_isPositive : val < 0 ? m_isNegative : m_isNeutral;
        if (m_state != res)
        {
            m_state = res;
            if (m_state)
            {
                m_eventTrue.Invoke();
            }
            else
            {
                m_eventFalse.Invoke();
            }
        }
    }
}
