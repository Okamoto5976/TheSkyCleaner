using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SkillType//ã≠âªÇçÏópÇ∑ÇÈç€ÅAéÌóﬁì¡íËÇÃÇΩÇﬂ
{
    Arm_PowerUP,
    NetUP,
    ShotUP,
    PlayerHealthUP,
    PlayerHealtDelect,
    InventoryUP,

    Count
}

[CreateAssetMenu(menuName = "Scriptable Objects/Data/SkillSO")]
public class SkillSO : ScriptableObject
{
    [SerializeField] private SkillDataSO m_skilldata;
    [SerializeField] private SkillType m_skilltype;
    [SerializeField] private string m_skillname;
    [SerializeField] private float m_updataValue;
    [SerializeField] private int m_id;

    [SerializeField] private SkillSO[] m_needskill;

    [SerializeField] private List<DropMaterial> m_materials;//DropSOÇÃíÜ

    public SkillType SkillType { get => m_skilltype;}
    public string Skillname { get => m_skillname; }
    public float UpdataValue { get => m_updataValue; }
    public int ID { get => m_id; }

    public SkillSO[] NeedSkill { get => m_needskill; }

    public SkillDataSO SkillData { get => m_skilldata; }
    public IReadOnlyList<DropMaterial> Materials => m_materials;

#if UNITY_EDITOR
    public void Initialize(SkillDataSO skilldata)
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
