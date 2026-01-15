using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{

    [SerializeField] private List<SkillSO> _skillSO = new List<SkillSO>();

    public List<SkillSO> SkillSO { get => _skillSO; set => _skillSO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        SkillSO skillSO = ScriptableObject.CreateInstance<SkillSO>();
        skillSO.name = "New Skill Type";
        skillSO.Initialise(this);

        _skillSO.Add(skillSO);

        AssetDatabase.AddObjectToAsset(skillSO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(skillSO);
    }
#endif
}
