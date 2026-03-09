using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/TrashPhase")]
public class TrashPhase : GamePhase
{
    [SerializeField] private PhaseType m_phase;

    public override void OnEnter()
    {
        switch (m_phase)
        {
            case PhaseType.None:
                break;

            case PhaseType.Start:
                m_gm.StartTrashPool();
                break;

            case PhaseType.Stop:
                m_gm.StopTrashPool();
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
