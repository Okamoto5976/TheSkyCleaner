using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    [SerializeField] private HealthContainer m_playerHealthContainer;

    [SerializeField] private List<ParticleSystem> m_onDamageParticles;

    private void OnEnable()
    {
        m_playerHealthContainer.ResetHealth();

        m_playerHealthContainer.OnDamage += OnDamage;
    }

    private void OnDisable()
    {
        m_playerHealthContainer.OnDamage -= OnDamage;
    }

    private void OnDamage()
    {
        Debug.Log("Play Particles");
        foreach (var particle in m_onDamageParticles)
        {
            particle.Play();
        }
    }
}
