using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [SerializeField] private InputContainer InputContainer;

    [SerializeField] private ParticleSystem[] m_boostParticles;

    private void OnEnable()
    {
        InputContainer.StrongAction.OnPress.OnTrigger += OnSpeedUp;
        InputContainer.StrongAction.OnRelease.OnTrigger += OnSpeedUpRelease;
    }

    private void OnDisable()
    {
        InputContainer.StrongAction.OnPress.OnTrigger -= OnSpeedUp;
        InputContainer.StrongAction.OnRelease.OnTrigger -= OnSpeedUpRelease;
    }

    private void OnSpeedUp()
    {
        foreach (var particle in m_boostParticles)
        {
            particle.Play();
        }
    }

    private void OnSpeedUpRelease()
    {
        foreach (var particle in m_boostParticles)
        {
            particle.Stop();
        }
    }
}
