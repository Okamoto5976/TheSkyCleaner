using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private Vector3 m_speeds;
    [SerializeField] private float m_speedMod = 1;

    private Transform m_transform;

    private void Awake()
    {
        m_transform = gameObject.transform;
    }

    public void MoveHorizontal(float dir) //•½sˆÚ“® X
    {
        Vector3 vel = dir * m_speeds.x * m_speedMod * Time.deltaTime * m_transform.right;
        m_transform.Translate(vel);
    }

    public void MoveVertical(float dir) //‚’¼ˆÚ“® Y
    {
        Vector3 vel = dir * m_speeds.y * m_speedMod * Time.deltaTime * m_transform.up;
        m_transform.Translate(vel);
    }

    public void MoveDepthical(float dir) //•½sˆÚ“® Z
    {
        Vector3 vel = dir * m_speeds.z * m_speedMod * Time.deltaTime * m_transform.forward;
        m_transform.Translate(vel);
    }

    public void SetSpeed(float val) //ˆÚ“®‘¬“x
    {
        m_speedMod = val;
    }
}
