using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Skillget : MonoBehaviour
{
    [SerializeField] private List<SkillSO> m_allSkill;
    [SerializeField] private List<SkillSO> m_unlockSkills;//取得したスキルの管理
    [SerializeField] private TextMeshProUGUI m_cost;
    [SerializeField] private SaveManager m_saveManager;

    //private int m_arm_powerup;
    //private int m_speedup;

    [SerializeField] private InventorySO m_inventorySO;

    //private int m_mycost = 20;//何らかの形で取得 仮置き

    private void Start()//初期化
    {
        
    }

    private void Update()
    {
        m_cost.text = BuildText();
        //m_cost.text = string.Format("NowPoint:{0}", m_mycost);
    }
    private string BuildText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("MY Material\n");

        foreach (var mat in m_inventorySO.GetAll())
        {
            sb.Append($"{mat.Key} × {mat.Value}\n");
        }

        return sb.ToString();
    }

    private bool CanUnlock(SkillSO skillData)
    {
        if (m_unlockSkills.Contains(skillData))
        {
            Debug.Log("取得済み");
            return false;
        }

        foreach(var need in skillData.NeedSkill)//必要なスキルを取得済みかどうか
        {
            if (!m_unlockSkills.Contains(need))
            {
                Debug.Log("解放されていません。");
                return false;
            }
        }

        if (!HasMaterials(skillData))
        {
            Debug.Log("ポイントが足りません");
            return false;//後々糸や布や
        }
        //m_mycost -= skillData.Cost;
        RemoveInventory(skillData);

        Debug.Log("取得");
        return true;
    }

    private bool HasMaterials(SkillSO skillData)
    {
        foreach(var need in skillData.Materials)
        {
            int have = m_inventorySO.Get(need.type);
            if(have < need.amount)
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveInventory(SkillSO skillData)
    {
        foreach (var need in skillData.Materials)
        {
            m_inventorySO.Remove(need.type, need.amount);
        }
    }

    public void Unlock(SkillSO skillData)//ボタンで呼ぶ
    {
        if(CanUnlock(skillData))
        {
            m_unlockSkills.Add(skillData);
        }

        //Adapt();//とりあえず
    }

    public void SaveSkillType()
    {
        m_saveManager.ResetEnhance();

        List<int> types = new List<int>();

        foreach (var skill in m_unlockSkills)
        {
            types.Add(skill.ID);
        }

        m_saveManager.EnhanceSave(types);
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
    }

    //private void Adapt()//適応したい際に呼ぶ
    //{
    //    foreach(var skill in unlockSkills)
    //    {
    //        switch (skill.SkillType)
    //        {
    //            case SkillType.Arm_PowerUP:
    //                m_arm_powerup = (int)skill.UpdataValue;
    //                Debug.Log("powerup" + m_arm_powerup);
    //                break;
    //            case SkillType.SpeedUP:
    //                m_speedup = (int)skill.UpdataValue;
    //                Debug.Log("speedup" + m_speedup);
    //                break;
    //            case SkillType.NetUP:
    //                break;
    //        }
    //    }
    //}

    //考え
    //取得したさい色を変えたい

    //unlockSkillsに入っているValueを値に移す　その値を使って強化
    //
    //Skillget内
    //int PlayerPower = skillData.Value;(Arm_PowerUP_1) + skillData.Value(Arm_PowerUP_2);
    //int PlayerSpeed = skillData.Value;(SpeedUP_1);
    //
    //Player内
    //int m_PlayerPower = base + Skillget.PlayerPower;
    //int m_PlayerSpeed = base + Skillget.PlaeyrSpeed;

    //後の考え
    //SkillTypeのみ保存で取得し、MainシーンでSkillSOの配列からSkillTypeの値を取得の感じ
}