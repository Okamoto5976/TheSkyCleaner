using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{

    [SerializeField] private List<SkillSO> _skillSO = new List<SkillSO>();

    public List<SkillSO> SkillSO { get => _skillSO; set => _skillSO = value; }
}
