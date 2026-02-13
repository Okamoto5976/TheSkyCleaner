using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PhaseSequence m_sequence;

    public List<GamePhase> m_phases = new();
    private int m_currentIndex;
    private GamePhase m_currentPhase;

    private void Start()
    {
        foreach(var phase in m_sequence.m_phase)
        {
            var instance = Instantiate(phase);
            instance.Inject(this);
            m_phases.Add(instance);
        }

        NextPhase();
    }

    private void Update()
    {
        if (m_currentPhase == null) return;

        if(m_currentPhase.OnUpdate(Time.deltaTime))//true ‚ÅI—¹
        {
            m_currentPhase.OnExit();
            NextPhase();
        }
    }

    private void NextPhase()
    {
        if(m_currentIndex >= m_phases.Count)
        {
            return;
        }

        m_currentPhase = m_phases[m_currentIndex];
        m_currentIndex++;
        m_currentPhase.OnEnter();
    }
}
