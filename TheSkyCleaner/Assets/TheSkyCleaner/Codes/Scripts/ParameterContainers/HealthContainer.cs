using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthContainer", menuName = "Scriptable Objects/Parameter Containers/HealthContainer")]
public class HealthContainer : ScriptableObject
{
    [SerializeField] private BooleanContainer m_isInvulnerable;
    [SerializeField] private FloatContainer m_fuel;
    [SerializeField] private IntegerContainer m_maxHealth;
    public int Value => (int)Mathf.Ceil(m_fuel.Value);

    public event UnityAction OnDamage = delegate { };
    public event UnityAction OnHeal = delegate { };

    public int MaxHealth => m_maxHealth.Value;

    public void SetValue(float value) => m_fuel.SetValue(value);

    public void ResetHealth()
    {
        m_fuel.SetValue(m_maxHealth.Value);
    }

    public void Damage(int value)
    {
        if (m_isInvulnerable.Value) return;
        m_fuel.SetValue(Mathf.Max(m_fuel.Value - value, 0));
        OnDamage.Invoke();
    }

    public void Heal(int value)
    {
        m_fuel.SetValue(Mathf.Min(m_fuel.Value + value, m_maxHealth.Value));
        OnHeal.Invoke();
    }
}
