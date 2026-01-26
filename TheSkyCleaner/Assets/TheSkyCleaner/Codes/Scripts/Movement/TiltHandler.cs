using UnityEngine;

public class TiltHandler : MonoBehaviour
{
    [SerializeField] private Vector3 m_angles;
    [SerializeField] private float m_tiltSpeed;

    private Transform m_transform;
    private Vector3 m_angle;

    private void Awake()
    {
        m_transform = gameObject.transform;
    }

    public void TiltRoll(float dir)
    {
        m_angle.z = m_angles.z * dir;
        SetRotation(m_angle);
    }

    public void TiltPitch(float dir)
    {
        m_angle.x = m_angles.x * dir;
        SetRotation(m_angle);
    }

    public void TiltYaw(float dir)
    {
        m_angle.y = m_angles.y * dir;
        SetRotation(m_angle);
    }

    private void SetRotation(Vector3 angle)
    {
        m_transform.localRotation = Quaternion.Lerp(
            m_transform.localRotation,
            Quaternion.Euler(angle),
            Time.deltaTime * m_tiltSpeed
            );
    }
}
