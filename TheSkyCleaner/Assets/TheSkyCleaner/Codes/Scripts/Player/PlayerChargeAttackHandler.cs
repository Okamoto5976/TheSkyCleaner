using UnityEngine;
using UnityEngine.Events;

public class PlayerChargeAttackHandler : MonoBehaviour
{
    [SerializeField] private Vector2 m_range;
    [SerializeField] private float m_lerpDuration;
    [SerializeField] private ParticleSystem m_chargingParticle;
    [SerializeField] private ParticleSystem m_chargedParticle;
    [SerializeField] private ParticleSystem m_burstParticle;
    [SerializeField] private ChangeTextureOffset m_discTextureOffset;

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
            m_chargingParticle.Stop();
            m_chargedParticle.Play();
            m_state = 0;
        }
        m_discTextureOffset.SetXOffset(val);
    }

    public void StartCharge()
    {
        m_chargingParticle.Play();
        m_state = 1;
    }

    public void StopCharge()
    {
        m_state = 0;
    }

    public void ReleaseCharge()
    {
        if (m_lerpTime == 1)
        {
            m_chargedParticle.Stop();
            m_burstParticle.Play();
            ResetCharge();
        }
        else
        {
            m_chargingParticle.Stop();
            m_state = -1;
        }
    }

    public void ResetCharge()
    {
        m_state = 0;
        m_lerpTime = 0;
        m_discTextureOffset.SetXOffset(0);
    }
}
