using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Update–ˆ‚ÉŒÄ‚Ño‚µ
/// </summary>
public class ConstantFloatEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> m_onMove;
    private void Update()
    {
        m_onMove.Invoke(1);
    }
}
