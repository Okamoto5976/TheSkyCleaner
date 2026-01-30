using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatContainer", menuName = "Scriptable Objects/Parameter Containers/FloatContainer")]
public class FloatContainer : RuntimeScriptableObject
{
    [SerializeField] private float m_initialValue=100;//‚P‚O‚Oƒ_ƒ‚â‚Á‚½‚ç‚¢‚Á‚Ä
    [SerializeField] private float m_value;

    public float Value => m_value;
    public float InitialValue => m_initialValue;//‚±‚ê‚àƒ_ˆÈ‰º—ª
    public void SetValue(float value)
    {
        m_value = value;
    }

    protected override void OnReset() => m_value = m_initialValue;
}
