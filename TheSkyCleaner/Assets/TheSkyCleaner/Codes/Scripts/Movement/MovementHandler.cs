using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private Vector3 m_speeds = Vector3.one;
    [SerializeField] private float m_speedMod = 1;

    protected Transform m_transform;
    protected bool m_success;

    protected virtual void Awake()
    {
        m_transform = gameObject.transform;
        m_success = true;
    }

    protected virtual bool ApplyTranslate(Vector3 vector, Space space = Space.Self)
    {
        m_transform.Translate(vector, space);
        return m_success;
    }

    public virtual void SetPosition(Vector3 position)
    {
        m_transform.position = position;
    }
    /// <summary>
    /// Move horizontally in <b><i>local space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public bool MoveHorizontal(float velocity)
    {
        Vector3 vel = velocity * m_speeds.x * m_speedMod * Time.deltaTime * m_transform.right;
        return ApplyTranslate(vel);
    }
    /// <summary>
    /// Move vertically in <b><i>local space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public bool MoveVertical(float velocity)
    {
        Vector3 vel = velocity * m_speeds.y * m_speedMod * Time.deltaTime * m_transform.up;
        return ApplyTranslate(vel);
    }
    /// <summary>
    /// Move depthically in <b><i>local space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public bool MoveDepthical(float velocity)
    {
        Vector3 vel = velocity * m_speeds.z * m_speedMod * Time.deltaTime * m_transform.forward;
        return ApplyTranslate(vel);
    }
    /// <summary>
    /// Move along the x axis in <b><i>global space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public bool MoveAlongX(float velocity)
    {
        Vector3 vel = velocity * m_speeds.x * m_speedMod * Time.deltaTime * Vector3.right;
        return ApplyTranslate(vel);
    }
    /// <summary>
    /// Move along the y axis in <b><i>global space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public bool MoveAlongY(float velocity)
    {
        Vector3 vel = velocity * m_speeds.y * m_speedMod * Time.deltaTime * Vector3.up;
        return ApplyTranslate(vel);
    }
    /// <summary>
    /// Move along the z axis in <b><i>global space</i></b>
    /// </summary>
    /// <param name="velocity">The speed to move at</param>
    public bool MoveAlongZ(float velocity)
    {
        Vector3 vel = velocity * m_speeds.z * m_speedMod * Time.deltaTime * Vector3.forward;
        return ApplyTranslate(vel);
    }
    /// <summary>
    /// Move in <b><i>local space</i></b>
    /// </summary>
    /// <param name="dir">The direction to move in</param>
    public bool MoveAll(Vector3 dir)
    {
        dir = dir.normalized * dir.magnitude;
        bool a = MoveHorizontal(dir.x);
        bool b = MoveVertical(dir.y);
        bool c = MoveDepthical(dir.z);
        return a || b || c;
    }
    /// <summary>
    /// Move in <b><i>global space</i></b>
    /// </summary>
    /// <param name="dir">The direction to move in</param>
    public bool MoveAllGlobal(Vector3 dir)
    {
        dir = dir.normalized;
        Vector3 vel = new()
        {
            x = dir.x * m_speeds.x,
            y = dir.y * m_speeds.y,
            z = dir.z * m_speeds.z,
        };
        return ApplyTranslate(m_speedMod * Time.deltaTime * vel, Space.World);
    }

    public bool MoveOnZ(Vector2 dir)
    {
        dir = dir.normalized * dir.magnitude;
        bool a = MoveHorizontal(dir.x);
        bool b = MoveVertical(dir.y);
        return a || b;
    }

    public bool MoveOnY(Vector2 dir)
    {
        dir = dir.normalized * dir.magnitude;
        bool a = MoveHorizontal(dir.x);
        bool b = MoveDepthical(dir.y);
        return a || b;
    }

    public bool MoveOnX(Vector2 dir)
    {
        dir = dir.normalized * dir.magnitude;
        bool a = MoveVertical(dir.y);
        bool b = MoveDepthical(dir.x);
        return a || b;
    }

    public void SetSpeed(float val)
    {
        m_speedMod = val;
    }
}
