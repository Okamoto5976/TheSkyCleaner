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
        m_value = Mathf.Lerp(m_value, dir, Time.deltaTime * m_lerpTime); //ŒÄ‚Ño‚³‚ê‚½ŠÖ”‚²‚Æ‚Ì’l‚Ædir‚ÌŠÔ‚ÅdeltaTime * lerpTime ‚É‚æ‚éüŒ`•âŠÔ
        m_event.Invoke(m_value);
    }
}
