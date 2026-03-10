using System.Collections.Generic;
using UnityEngine;

public class SkillAdapt : MonoBehaviour
{
    [SerializeField] private SkillDataSO m_skilldata; //スキル取得できるものすべて
    [SerializeField] private List<SkillSO> m_unlockSkills;//取得したスキルの管理
    [SerializeField] private SaveManager m_saveManager;
    [SerializeField] private ResultScreen m_result;

    private int m_arm_powerup;
    private int m_speedup;

    public int Arm_PowerUP { get => m_arm_powerup; }
    public int SpeedUP { get => m_speedup; }

    public void Adapt()//適応したい際に呼ぶ
    {
        foreach (var skill in m_unlockSkills)
        {
            switch (skill.SkillType)
            {
                case SkillType.Arm_PowerUP:
                    m_arm_powerup += (int)skill.UpdataValue;
                    Debug.Log("powerup" + m_arm_powerup);

                    m_result.m_enhanceLevel[0] += 1;
                    break;
                case SkillType.NetUP:
                    m_speedup += (int)skill.UpdataValue;
                    Debug.Log("speedup" + m_speedup);

                    m_result.m_enhanceLevel[1] += 1;
                    break;
                case SkillType.ShotUP:

                    m_result.m_enhanceLevel[2] += 1;
                    break;
                case SkillType.PlayerHealthUP:
                    m_result.m_enhanceLevel[3] += 1;

                    break;
                case SkillType.PlayerHealtDelect:
                    m_result.m_enhanceLevel[4] += 1;

                    break;
                case SkillType.InventoryUP:
                    m_result.m_enhanceLevel[5] += 1;

                    break;
            }
        }
    }

    public void LoadSkillType()
    {
        EnhanceList data = m_saveManager.EnhanceLoad();
        if (data == null) return;

        m_unlockSkills.Clear();

        foreach (var id in data.m_unlockSkills)
        {
            SkillSO found = m_skilldata.SkillSO
                .Find(s => s.ID == id);

            if (found != null)
                m_unlockSkills.Add(found);
        }

        Adapt();
    }
}
