using UnityEngine;

public class BoundMovementHandler : MovementHandler
{
    [SerializeField] private AxisVector3Container m_boundSize;
    [SerializeField] private Vector3 m_center;

    private Bounds m_bounds;

    protected override void Awake()
    {
        base.Awake();
        m_bounds = new(m_center, m_boundSize.Value);
        Debug.Log($"{m_bounds.min} - {m_bounds.max}");
    }

    protected override bool ApplyTranslate(Vector3 vector, Space space = Space.Self)
    {
        if (IsInBounds(m_transform.position + vector))
        {
            return base.ApplyTranslate(vector, space);
        }
        Vector3 newPos = m_transform.position + vector ;
        newPos.x = Mathf.Clamp(newPos.x, -m_bounds.extents.x + m_bounds.center.x, m_bounds.extents.x + m_bounds.center.x);
        newPos.y = Mathf.Clamp(newPos.y, -m_bounds.extents.y + m_bounds.center.y, m_bounds.extents.y + m_bounds.center.y);
        newPos.z = Mathf.Clamp(newPos.z, -m_bounds.extents.z + m_bounds.center.z, m_bounds.extents.z + m_bounds.center.z);
        m_transform.position = newPos;
        return true;
    }

    private bool IsInBounds(Vector3 to)
    {
        if (m_bounds.Contains(to))
        {
            return true;
        }
        return false;
    }
}
