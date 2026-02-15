using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/BossPhase")]
public class BossPhase : GamePhase
{
    [SerializeField] private PhaseType m_phase;

    public override void OnEnter()
    {
        switch(m_phase)
        {
            case PhaseType.None:
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
