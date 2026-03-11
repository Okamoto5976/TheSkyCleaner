using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/EnemyPhase")]
public class EnemyPhase : GamePhase
{
    [SerializeField] private PhaseType m_phase;

    public override void OnEnter()
    {
        switch (m_phase)
        {
            case PhaseType.None:
                break;

            case PhaseType.Start:
                m_gm.StartEnemyPool();
                break;

            case PhaseType.Stop:
                m_gm.StopEnemyPool();
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
