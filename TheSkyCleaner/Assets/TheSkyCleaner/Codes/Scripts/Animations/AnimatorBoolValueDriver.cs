using UnityEngine;

public class AnimatorBoolValueDriver : MonoBehaviour
{
    [SerializeField] private string m_name;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Drive(bool value)
    {
        m_animator.SetBool(m_name, value);
    }
}
