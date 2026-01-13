using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject
{
    [SerializeField] private SkillDataSO m_skilldata;
    [SerializeField] private string m_skillname;
    [SerializeField] private float m_updataValue;

    public string Skillname { get => m_skillname; }
    public float UpdataValue { get => m_updataValue; }

    public SkillDataSO SkillData { get => m_skilldata;}

}
