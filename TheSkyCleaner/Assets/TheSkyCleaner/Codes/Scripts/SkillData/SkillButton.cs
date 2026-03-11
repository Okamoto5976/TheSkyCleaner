using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public SkillSO m_skill; // Åö
    [SerializeField] private Skillget m_skillget;
    [SerializeField] private Image m_lock;
    [SerializeField] private TextMeshProUGUI m_cost;


    private void Start()
    {

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

    private void Update()
    {
        if(m_skillget.UnlockSkills.Contains(m_skill))
        {
            m_lock.color = Color.yellow;
        }
    }

    public void OnClick()
    {
        Debug.Log("ButtonPress");
        m_skillget.Unlock(m_skill);
    }
}
