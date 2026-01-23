using TMPro;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private SkillSO m_skill;
    [SerializeField] private Skillget m_skillget;
    [SerializeField] private TextMeshProUGUI m_info;
    [SerializeField] private TextMeshProUGUI m_name;
    [SerializeField] private TextMeshProUGUI m_cost;


    private void Start()
    {
        m_info.text = m_skill.Info;
        m_name.text = m_skill.Skillname;
        m_cost.text = string.Format("Point:{0}", m_skill.Cost);
    }

    public void OnClick()
    {
        m_skillget.Unlock(m_skill);
    }
}
