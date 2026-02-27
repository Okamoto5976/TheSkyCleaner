using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

[CreateAssetMenu(fileName = "BossDirectAttackState", menuName = "Scriptable Objects/Boss/States/Direct Attack State")]
public class BossDirectAttackState : BossStateBase
{
    [SerializeField] private StringContainer m_animationName;
    [SerializeField] private float m_rampTime;
    [SerializeField] private float m_attackTime;
    [SerializeField] private List<Rect> m_attackAreas;
    [SerializeField] private int m_attackStrength;
    [SerializeField] private float m_endTime;

    public override float EnterAction(BossController controller)
    {
        m_actionIndex = 0;
        m_isStateEnd = false;
        if (m_animationName != null)
        {
            controller.PlayAnimation(m_animationName.Value);
        }
        return m_rampTime;
    }
    public override float GetActionTime()
    {
        return m_actionIndex switch
        {
            0 => m_rampTime,
            1 => m_attackTime,
            2 => m_endTime,
            _ => 0,
        };
    }

    public override bool DoAction(BossController bossController)
    {
        switch (m_actionIndex)
        {
            case 0: break;
            case 1: DoAttack(bossController); break;
            case 2: break;
            case 3: break;
        }
        return m_isStateEnd;
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

    private void DoAttack(BossController bossController)
    {
        foreach (var attackArea in m_attackAreas)
        {
            if (attackArea.Contains(bossController.PlayerPosition))
            {
                bossController.PlayerHealth.Damage(m_attackStrength);
            }
        }
    }

}
