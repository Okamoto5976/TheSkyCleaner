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

    public void MoveHorizontal(float dir)
    {
        Vector3 vel = dir * m_speeds.x * m_speedMod * Time.deltaTime * m_transform.right;
        m_transform.Translate(vel);
    }

    public void MoveVertical(float dir)
    {
        Vector3 vel = dir * m_speeds.y * m_speedMod * Time.deltaTime * m_transform.up;
        m_transform.Translate(vel);
    }

    public void MoveDepthical(float dir)
    {
        Vector3 vel = dir * m_speeds.z * m_speedMod * Time.deltaTime * m_transform.forward;
        m_transform.Translate(vel);
    }

    public void MoveAll(Vector3 dir)
    {
        dir = dir.normalized * dir.magnitude;
        MoveHorizontal(dir.x);
        MoveVertical(dir.y);
        MoveDepthical(dir.z);
    }

    public void MoveOnZ(Vector2 dir)
    {
        dir = dir.normalized * dir.magnitude;
        MoveHorizontal(dir.x);
        MoveVertical(dir.y);
    }

    public void MoveOnY(Vector2 dir)
    {
        dir = dir.normalized * dir.magnitude;
        MoveHorizontal(dir.x);
        MoveDepthical(dir.y);
    }

    public void MoveOnX(Vector2 dir)
    {
        dir = dir.normalized * dir.magnitude;
        MoveVertical(dir.y);
        MoveDepthical(dir.x);
    }

    public void SetSpeed(float val)
    {
        m_speedMod = val;
    }
}
