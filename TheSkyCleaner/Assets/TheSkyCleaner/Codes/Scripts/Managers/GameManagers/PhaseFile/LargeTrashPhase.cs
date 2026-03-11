using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/LargeTrashPhase")]
public class LargeTrashPhase : GamePhase
{
    [SerializeField] private PhaseType m_phase;

    public override void OnEnter()
    {
        switch (m_phase)
        {
            case PhaseType.None:
                break;

            case PhaseType.StartPool:
                m_gm.StartLargeTrashPool();
                break;

            case PhaseType.StopPool:
                m_gm.StopLargeTrashPool();
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
