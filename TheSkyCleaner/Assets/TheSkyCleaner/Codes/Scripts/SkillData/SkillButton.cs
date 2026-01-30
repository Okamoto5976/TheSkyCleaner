using System.Text;
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
        m_cost.text = BuildText();
        //m_cost.text = string.Format("Point:{0}", m_skill.Cost);
    }

    private string BuildText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Need\n");

        foreach (var mat in m_skill.Materials)
        {
            sb.Append($"{mat.type} Å~ {mat.amount}\n");
        }

        return sb.ToString();
    }

    public void OnClick()
    {
        m_skillget.Unlock(m_skill);
    }
}
