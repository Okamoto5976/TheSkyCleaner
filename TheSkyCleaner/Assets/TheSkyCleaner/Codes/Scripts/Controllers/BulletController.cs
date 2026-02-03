using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private MovementHandler m_MovementHandler;
    [SerializeField] private ReturnObjectToPool m_ReturnObjectToPool;

    private Vector3 m_direction;
    private float m_velocity;

    public void InjectDirection(Vector3 direction) => m_direction = direction;
    public void InjectVelocity(float velocity) => m_velocity = velocity;

    public void Initialize()
    {
        m_MovementHandler.SetSpeed(m_velocity);
        transform.rotation = Quaternion.LookRotation(m_direction);
    }

    private void Update()
    {
        m_MovementHandler.MoveAllGlobal(m_direction);
    }
}
