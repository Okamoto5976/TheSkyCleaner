using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Skillget : MonoBehaviour
{
    public List<SkillSO> unlockSkills;//取得したスキルの管理 ★
    [SerializeField] private TextMeshProUGUI m_cost;

    private int m_arm_powerup;
    private int m_speedup;

    [SerializeField] private InventorySO m_inventorySO;
    [SerializeField] private UpgradeScreen m_upgradeScreen;
    [SerializeField] private List<ButtonAnimation> m_buttonAnimations;

    private int m_mycost = 20;//何らかの形で取得 仮置き

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
        if (unlockSkills.Contains(skillData))
        {
            Debug.Log("取得済み");
            return false;
        }

        foreach(var need in skillData.NeedSkill)//必要なスキルを取得済みかどうか
        {
            if (!unlockSkills.Contains(need))
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

        // 取得したらボタンの状態を変える
        for (int i = 0; i < (int)UpgradeButtonType.ButtonAmount; i++)
        {
            m_upgradeScreen.CheckUnlock(i, m_upgradeScreen.m_skills[i - 1]);
            m_buttonAnimations[i].ButtonStateUpdate();
        }

        Debug.Log("取得");
        return true;
    }

    public bool HasMaterials(SkillSO skillData) // ★
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
            unlockSkills.Add(skillData);
        }

        Adapt();//とりあえず
    }

    private void Adapt()//適応したい際に呼ぶ
    {
        foreach(var skill in unlockSkills)
        {
            switch (skill.SkillType)
            {
                case SkillType.Arm_PowerUP:
                    m_arm_powerup = (int)skill.UpdataValue;
                    Debug.Log("powerup" + m_arm_powerup);
                    break;
                case SkillType.SpeedUP:
                    m_speedup = (int)skill.UpdataValue;
                    Debug.Log("speedup" + m_speedup);
                    break;
                case SkillType.NetUP:
                    break;
            }
        }
    }

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
}
