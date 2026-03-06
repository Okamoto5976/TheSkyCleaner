using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    [SerializeField] private HealthContainer m_playerHealthContainer;

    [SerializeField] private List<ParticleSystem> m_onDamageParticles;

    private BooleanContainer IsDamageInvulnerable => m_playerHealthContainer.IsDamageInvulnerable;

    private WaitForSeconds m_sleep;

    private void Awake()
    {
        m_sleep = new(m_playerHealthContainer.InvulnerabilityTime);
    }
    private void OnEnable()
    {
        m_playerHealthContainer.ResetHealth();

        m_playerHealthContainer.OnDamage += OnDamage;
    }

    private void OnDisable()
    {
        m_playerHealthContainer.OnDamage -= OnDamage;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnDamage()
    {
        foreach (var particle in m_onDamageParticles)
        {
            particle.Play();
        }
        StartCoroutine(OnInvulnerability());
    }

    private IEnumerator OnInvulnerability()
    {
        IsDamageInvulnerable.SetValue(true);
        yield return m_sleep;
        IsDamageInvulnerable.SetValue(false);
    }
}
