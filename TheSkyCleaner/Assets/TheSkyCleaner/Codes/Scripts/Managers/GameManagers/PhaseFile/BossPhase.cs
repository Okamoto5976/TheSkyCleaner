using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/BossPhase")]
public class BossPhase : GamePhase
{
    [SerializeField] private PhaseType m_phase;

    [SerializeField] private TriggerContainer m_bossActive;
    [SerializeField] private TriggerContainer m_bossDeactivate;

    public override void OnEnter()
    {
        switch (m_phase)
        {
            case PhaseType.Start:
                m_bossActive.Trigger();
                break;
            case PhaseType.Stop:
                m_bossDeactivate.Trigger();

                break;
        }

        
    }

    public override bool OnUpdate(float deltaTime)
    {
        


        return true;
    }

    public override void OnExit()
    {
        
    }
}
