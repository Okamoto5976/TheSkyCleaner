using UnityEngine;
using UnityEngine.UI;

public class T_BossHealth : MonoBehaviour
{
    [SerializeField] private Image m_slider;
    [SerializeField] private HealthContainer m_bossHealth;

    private void Update()
    {
        //m_slider.maxValue = m_bossHealth.MaxHealth;
        //m_slider.value = m_bossHealth.Value;

        m_slider.fillAmount = m_bossHealth.Value / m_bossHealth.MaxHealth;
    }
}
