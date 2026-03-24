using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    [SerializeField] private HealthContainer m_playerHealthContainer;

    [SerializeField] private List<ParticleSystem> m_onDamageParticles;
    [SerializeField] private AudioContainer m_hitSound;
    [SerializeField] private List<ParticleSystem> m_onHealParticles;
    [SerializeField] private AudioContainer m_healSound;
    [SerializeField] private AudioSource m_audioSource;

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
        m_playerHealthContainer.OnHeal += OnHeal;
    }

    private void OnDisable()
    {
        m_playerHealthContainer.OnDamage -= OnDamage;
        m_playerHealthContainer.OnHeal -= OnHeal;
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
        m_audioSource.PlayOneShot(m_hitSound.AudioClip, m_hitSound.Volume);
        StartCoroutine(OnInvulnerability());
    }

    private void OnHeal()
    {
        foreach (var particle in m_onHealParticles)
        {
            particle.Play();
        }
        m_audioSource.PlayOneShot(m_healSound.AudioClip, m_healSound.Volume);
    }

    private IEnumerator OnInvulnerability()
    {
        IsDamageInvulnerable.SetValue(true);
        yield return m_sleep;
        IsDamageInvulnerable.SetValue(false);
    }
}
