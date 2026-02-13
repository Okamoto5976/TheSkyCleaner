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
        return false;
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
