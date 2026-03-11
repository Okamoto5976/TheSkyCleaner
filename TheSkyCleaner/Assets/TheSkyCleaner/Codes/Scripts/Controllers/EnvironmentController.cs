using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private Vector3 m_direction;
    [SerializeField] private FloatContainer m_speedContainer;
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private float m_loopStart;
    [SerializeField] private float m_loopEnd;

    private void FixedUpdate()
    {
        m_movementHandler.SetSpeed(m_speedContainer.Value);
        m_movementHandler.MoveAllGlobal(m_direction);
        if (transform.position.z < m_loopEnd)
        {
            Vector3 pos = transform.position;
            pos.z = m_loopStart;
            m_movementHandler.SetPosition(pos);
        }
    }
}
