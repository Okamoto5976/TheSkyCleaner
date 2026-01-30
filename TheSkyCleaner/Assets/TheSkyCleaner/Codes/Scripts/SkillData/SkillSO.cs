using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SkillType//強化を作用する際、種類特定のため
{
    Arm_PowerUP,
    SpeedUP,
    NetUP
}

[CreateAssetMenu(menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject
{
    [SerializeField] private SkillDataSO m_skilldata;
    [SerializeField] private SkillType m_skilltype;
    [SerializeField] private string m_skillname;
    [SerializeField] private float m_updataValue;

    [SerializeField] private Sprite m_icon;
    [SerializeField] private int m_cost;
    [SerializeField] private string m_info;
    [SerializeField] private SkillSO[] m_needskill;

    [SerializeField] private List<DropMaterial> m_materials;//DropSOの中

    public SkillType SkillType { get => m_skilltype;}
    public string Skillname { get => m_skillname; }
    public float UpdataValue { get => m_updataValue; }//変更値
    //取得例float value = skillDataSO.SkillSO[0].UpdataValue;

    public Sprite Icon { get => m_icon; }

    public int Cost { get => m_cost; }

    public string Info { get => m_info; }

    public SkillSO[] NeedSkill { get => m_needskill; }

    public SkillDataSO SkillData { get => m_skilldata; }
    public IReadOnlyList<DropMaterial> Materials => m_materials;

#if UNITY_EDITOR
    public void Initialise(SkillDataSO skilldata)
    {
        m_skilldata = skilldata;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename to name")]
    private void Rename()
    {
        this.name = m_skillname;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete this")]
    private void DeleteThis()
    {
        m_skilldata.SkillSO.Remove(this);
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
