using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject
{
    [SerializeField] private SkillDataSO m_skilldata;
    [SerializeField] private string m_skillname;
    [SerializeField] private float m_updataValue;
    [SerializeField] private Material m_material;
    public string Skillname { get => m_skillname; }
    public float UpdataValue { get => m_updataValue; }//•ÏX’l
    //Žæ“¾—áfloat value = skillDataSO.SkillSO[0].UpdataValue;

    public SkillDataSO SkillData { get => m_skilldata; }

    [System.Serializable]
    public class Material
    {
        [Header("Ž…"), SerializeField] private int m_material_1;
        [Header("•z"), SerializeField] private int m_material_2;
    }

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
