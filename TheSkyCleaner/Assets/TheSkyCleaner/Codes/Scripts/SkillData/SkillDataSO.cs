using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    [SerializeField] private List<SkillSO> m_skillSO = new List<SkillSO>();

    public List<SkillSO> SkillSO { get => m_skillSO; set => m_skillSO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        SkillSO skillSO = ScriptableObject.CreateInstance<SkillSO>();
        skillSO.name = "New Skill Type";
        skillSO.Initialise(this);

        m_skillSO.Add(skillSO);

        AssetDatabase.AddObjectToAsset(skillSO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(skillSO);
    }
#endif
}
