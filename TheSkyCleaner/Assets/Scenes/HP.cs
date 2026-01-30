using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [Header("HP設定")]
    [SerializeField]private float m_maxHP = 100f;//フル
    [SerializeField]private float m_currentHP;//今のHP

    [Header("UI設定")]
    [SerializeField] private Slider m_HPba;//UI

    void Start()
    {
        m_currentHP = m_maxHP;
        UpdateUI();
    }
    public void Damage(float damage)
    {
        m_currentHP-=damage;
        m_currentHP = Mathf.Clamp(m_currentHP, 0, m_maxHP);
        
        Debug.Log($"痛った!?クソが 残りHP:{m_currentHP}");
        UpdateUI();

        if (m_currentHP <= 0) Unablefight();
    }
    private void UpdateUI()
    {
        if (m_HPba != null)
        {
            m_HPba.maxValue = m_maxHP;
            m_HPba.value = m_currentHP;
        }

    }
    private void Unablefight()
    {
        Debug.Log("掃除継続不能の破損です");
       // gameObject.SetActive(false);//まあ多分いらんか
    }
}
