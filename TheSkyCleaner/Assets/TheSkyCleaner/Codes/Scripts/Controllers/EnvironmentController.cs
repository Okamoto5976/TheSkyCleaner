using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private Vector3 m_direction;
    [SerializeField] private FloatContainer m_speedContainer;
    [SerializeField] private MovementHandler m_movementHandler;

    private void FixedUpdate()
    {
        m_movementHandler.SetSpeed(m_speedContainer.Value);
        m_movementHandler.MoveAllGlobal(m_direction);
    }
}
