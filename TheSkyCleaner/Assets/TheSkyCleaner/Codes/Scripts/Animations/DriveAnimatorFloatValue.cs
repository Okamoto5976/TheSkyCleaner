using UnityEngine;

public class DriveAnimatorFloatValue : MonoBehaviour
{
    [SerializeField] private string m_name;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Drive(float value)
    {
        m_animator.SetFloat(m_name, value);
    }
}
