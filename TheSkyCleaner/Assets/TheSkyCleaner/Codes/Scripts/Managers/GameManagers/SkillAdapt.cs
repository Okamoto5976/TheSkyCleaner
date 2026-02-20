using System.Collections.Generic;
using UnityEngine;

public class SkillAdapt : MonoBehaviour
{
    [SerializeField] private List<SkillSO> m_allSkill; //スキル取得できるものすべて
    [SerializeField] private List<SkillSO> m_unlockSkills;//取得したスキルの管理
    [SerializeField] private SaveManager m_saveManager;

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
                    break;
                case SkillType.SpeedUP:
                    m_speedup += (int)skill.UpdataValue;
                    Debug.Log("speedup" + m_speedup);
                    break;
                case SkillType.NetUP:
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
            SkillSO found = m_allSkill
                .Find(s => s.ID == id);

            if (found != null)
                m_unlockSkills.Add(found);
        }

        Adapt();
    }
}
