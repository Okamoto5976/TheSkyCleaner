using UnityEngine;

public class ActivateObjectAtPosition : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager m_objectPoolManager;
    [SerializeField] private bool m_isChild;
    [SerializeField] private bool m_isRelative;
    [SerializeField] private Vector3 m_offsetPosition;
    [SerializeField] private Vector3 m_offsetRotation;
    [SerializeField] private Vector3 m_offsetScale = Vector3.one;

    private Transform m_transform;

    private void Awake()
    {
        m_transform = transform;
    }

    public void Activate()
    {
        GameObject obj = m_objectPoolManager.GetFromPool(true);
        Transform t = obj.transform;
        Vector3 pos = m_offsetPosition;
        Vector3 rot = m_offsetRotation;

        if (m_isRelative)
        {
            pos += m_transform.localPosition;
            rot += m_transform.localEulerAngles;
        }

        if (m_isChild)
        {
            t.parent = m_transform;
            t.SetLocalPositionAndRotation(pos, Quaternion.Euler(rot));
        }
        else
        {
            t.SetPositionAndRotation(pos, Quaternion.Euler(rot));
        }

        t.localScale = m_offsetScale;
    }

    public void SetOffsetPosition(Vector3 pos)
    {
        m_offsetPosition = pos;
    }
}
