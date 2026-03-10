using UnityEngine;

[CreateAssetMenu(fileName = "BossMoveState", menuName = "Scriptable Objects/Boss/States/Move State")]
public class BossMoveState : BossStateBase
{
    [SerializeField] private float m_rampTime;
    [SerializeField] private Vector3 m_position;
    [SerializeField] private float m_moveTime;
    [SerializeField] private float m_endTime;

    private Vector3 m_previousPosition;

    public override bool DoAction(BossController bossController)
    {
        switch (m_actionIndex)
        {
            case 0: break;
            case 1: DoMove(bossController); break;
            case 2: break;
            case 3: break;
        }
        return m_isStateEnd;
    }

    private void DoMove(BossController bossController)
    {
        Debug.Log($"{bossController.StateTime}, {m_moveTime}, {bossController.StateTime / m_moveTime}");
        Vector3 pos = Vector3.Lerp(m_position, m_previousPosition, bossController.StateTime / m_moveTime);
        bossController.MovementHandler.SetPosition(pos);
    }

    

    public override float EnterAction(BossController bossController)
    {
        m_actionIndex = 0;
        m_isStateEnd = false;
        m_previousPosition = bossController.Transform.position;
        return m_rampTime;
    }

    public override float GetActionTime()
    {
        switch (m_actionIndex)
        {
            case 0: return m_rampTime;
            case 1: return m_moveTime;
            case 2: return m_endTime;
        }
        return 0;
    }

    protected override bool DoActionOneShot(BossController bossController)
    {
        switch (m_actionIndex)
        {
            case 0: break;
            case 1: break;
            case 2: break;
            case 3: m_isStateEnd = true; break;
        }
        return m_isStateEnd;
    }
}
