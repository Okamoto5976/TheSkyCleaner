using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Skillget : MonoBehaviour
{
    [SerializeField] private List<SkillSO> m_skilldata; //スキル取得できるものすべて
    [SerializeField] private List<SkillSO> m_unlockSkills;
    //public List<SkillSO> unlockSkills;//取得したスキルの管理 ★
    //[SerializeField] private TextMeshProUGUI m_cost;
    [SerializeField] private SaveManager m_saveManager;

    public List<SkillSO> UnlockSkills { get => m_unlockSkills; }

    //private int m_arm_powerup;
    //private int m_speedup;

    [SerializeField] private InventorySO m_inventorySO;
    //[SerializeField] private UpgradeScreen m_upgradeScreen;//★
    //[SerializeField] private List<ButtonAnimation> m_buttonAnimations;//★

    [SerializeField] private IntegerContainer m_armCount;
    [SerializeField] private IntegerContainer m_netCount;
    [SerializeField] private IntegerContainer m_shopDamage;
    [SerializeField] private IntegerContainer m_playerHealth;
    [SerializeField] private IntegerContainer m_inventoryCount;

    private void Start()//初期化
    {

    }

    private void Update()
    {
        //m_cost.text = BuildText();
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
        RemoveInventory(skillData);

        //// 取得したらボタンの状態を変える
        //for (int i = 0; i < (int)UpgradeButtonType.ButtonAmount; i++)
        //{
        //    m_upgradeScreen.CheckUnlock(i, m_upgradeScreen.m_skills[i - 1]);
        //    m_buttonAnimations[i].ButtonStateUpdate();
        //}

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
            m_unlockSkills.Add(skillData);
            Adapt(skillData);//とりあえず
        }
    }

    public void Adapt(SkillSO skill)//適応したい際に呼ぶ
    {
        switch (skill.SkillType)
        {
            case SkillType.Arm_CountUP:
                {
                    int armCount = m_armCount.Value + (int)skill.UpdataValue;
                    m_armCount.SetValue(armCount);
                    break;
                }
            case SkillType.NetUP:
                {
                    int netCount = m_netCount.Value + (int)skill.UpdataValue;
                    m_netCount.SetValue(netCount);
                    break;
                }
            case SkillType.ShotUP:

                break;
            case SkillType.PlayerHealthUP:
                {
                    int playerHealth = m_playerHealth.Value + (int)skill.UpdataValue;
                    m_playerHealth.SetValue(playerHealth);
                    break;
                }
            case SkillType.PlayerHealtDelect:
                {
                    //int shotDamage = m_shopDamage.Value + (int)skill.UpdataValue;
                    //m_shopDamage.SetValue(shotDamage);
                    break;
                }
            case SkillType.InventoryUP:
                {
                    int inventoryCount = m_inventoryCount.Value + (int)skill.UpdataValue;
                    m_inventoryCount.SetValue(inventoryCount);
                    break;
                }

        }

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
        Debug.Log("load");
        foreach (var id in data.m_unlockSkills)
        {
            SkillSO found = m_skilldata
               .Find(s => s.ID == id);

            if (found != null)
            {
                m_unlockSkills.Add(found);
                Debug.Log("unlockSkill");
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
