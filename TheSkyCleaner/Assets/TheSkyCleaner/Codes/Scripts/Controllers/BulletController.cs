using UnityEngine;

[RequireComponent(typeof(MovementHandler), typeof(ReturnObjectToPool))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private FloatContainer m_shotCollisionDistance;
    [SerializeField] private FloatContainer m_shotMaximumDistance;
    [SerializeField] private IntegerContainer m_shotDamage;
    private MovementHandler m_movementHandler;
    private ReturnObjectToPool m_returnObjectToPool;
    

    private Vector3 m_direction;
    private IDamage m_targetEnemy;
    private float m_velocity;
    private Vector3 m_origin;
    private bool m_isTargetAlive;

    private Transform m_transform;

    public void InjectDirection(Vector3 direction) => m_direction = direction;
    public void InjectTarget(IDamage target)
    {
        if (target == null)
        {
            m_targetEnemy = null;
        }
        else
        {
            m_targetEnemy = target;
            m_isTargetAlive = true;
        }
    }

    public void InjectVelocity(float velocity) => m_velocity = velocity;

    private void Awake()
    {
        m_movementHandler = GetComponent<MovementHandler>();
        m_returnObjectToPool = GetComponent<ReturnObjectToPool>();
        m_transform = transform;
    }

    public void Initialize(Vector3 pos)
    {
        m_movementHandler.SetSpeed(m_velocity);
        m_origin = pos;
        m_transform.position = pos;
    }

    private void Update()
    {
        Move();
        CheckCollision();
        CheckDistance();
    }

    private void UpdateDirection()
    {
        if (!IsTargetAlive()) return;

        m_direction = (m_targetEnemy.Transform.position - m_transform.position).normalized;
    }

    private void Move()
    {
        UpdateDirection();
        m_transform.rotation = Quaternion.LookRotation(m_direction);
        m_movementHandler.MoveAllGlobal(m_direction);
    }

    private void CheckCollision()
    {
        if (IsColliding())
        {
            m_targetEnemy.Damage(m_shotDamage.Value);
            m_returnObjectToPool.ReturnToPool();
        }
    }

    private bool IsColliding()
    {
        if (!IsTargetAlive()) return false;

        float dist = (m_targetEnemy.Transform.position - m_transform.position).magnitude;
        if (dist < m_shotCollisionDistance.Value) return true;
        return false;
    }

    private bool IsTargetAlive()
    {
        if (m_targetEnemy == null) return false;
        if (!m_isTargetAlive) return false;
        if (!m_targetEnemy.IsActive)
        {
            m_isTargetAlive = false;
            return false;
        }
        return true;
    }

    private void CheckDistance()
    {
        float dist = (m_transform.position - m_origin).magnitude;
        if (dist >= m_shotMaximumDistance.Value)
        {
            m_returnObjectToPool.ReturnToPool();
        }
    }
}
