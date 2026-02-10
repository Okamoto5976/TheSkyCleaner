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

    /// <summary>
    /// Move horizontally in <b><i>local space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public void MoveHorizontal(float velocity)
    {
        Vector3 vel = velocity * m_speeds.x * m_speedMod * Time.deltaTime * m_transform.right;
        m_transform.Translate(vel);
    }
    /// <summary>
    /// Move vertically in <b><i>local space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public void MoveVertical(float velocity)
    {
        Vector3 vel = velocity * m_speeds.y * m_speedMod * Time.deltaTime * m_transform.up;
        m_transform.Translate(vel);
    }
    /// <summary>
    /// Move depthically in <b><i>local space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public void MoveDepthical(float velocity)
    {
        Vector3 vel = velocity * m_speeds.z * m_speedMod * Time.deltaTime * m_transform.forward;
        m_transform.Translate(vel);
    }
    /// <summary>
    /// Move along the x axis in <b><i>global space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public void MoveAlongX(float velocity)
    {
        Vector3 vel = velocity * m_speeds.x * m_speedMod * Time.deltaTime * Vector3.right;
        m_transform.Translate(vel);
    }
    /// <summary>
    /// Move along the y axis in <b><i>global space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public void MoveAlongY(float velocity)
    {
        Vector3 vel = velocity * m_speeds.y * m_speedMod * Time.deltaTime * Vector3.up;
        m_transform.Translate(vel);
    }
    /// <summary>
    /// Move along the z axis in <b><i>global space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public void MoveAlongZ(float velocity)
    {
        Vector3 vel = velocity * m_speeds.z * m_speedMod * Time.deltaTime * Vector3.forward;
        m_transform.Translate(vel);
    }
    /// <summary>
    /// Move in <b><i>local space</i></b>
    /// </summary>
    /// <param name="dir">The direction to move in</param>
    public void MoveAll(Vector3 dir)
    {
        dir = dir.normalized * dir.magnitude;
        MoveHorizontal(dir.x);
        MoveVertical(dir.y);
        MoveDepthical(dir.z);
    }
    /// <summary>
    /// Move in <b><i>global space</i></b>
    /// </summary>
    /// <param name="dir">The direction to move in</param>
    public void MoveAllGlobal(Vector3 dir)
    {
        dir = dir.normalized;
        Vector3 vel = new()
        {
            x = dir.x * m_speeds.x,
            y = dir.y * m_speeds.y,
            z = dir.z * m_speeds.z,
        };
        m_transform.position += m_speedMod * Time.deltaTime * vel;
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
