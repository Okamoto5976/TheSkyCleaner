using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class TrashController : MonoBehaviour, ILockOnTarget
{
    [SerializeField] private CollectSO m_collectSO;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private FloatContainer m_fuel;

    [Header("Refalence")]
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private ReturnObjectToPool m_returnObjectToPool;

    [Header("Movement")]
    [System.NonSerialized] public float m_moveSpeed;
    [System.NonSerialized] public Vector3 m_direction;

    private SphereCollider m_collider;

    private Vector3 m_initDir;

    private bool m_isMove = false;

    private Transform m_transform;
    private int m_attack;
    public Transform Transform => m_transform;

    public GameObject GameObject => gameObject;

    public DropSO GetDropData()=> m_dropSO;

    private void Awake()
    {
        m_collider = gameObject.GetComponent<SphereCollider>();
        m_transform = transform;
    }

    private void OnEnable()
    {
        m_attack = m_collectSO.Attack;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        m_moveSpeed = moveSpeed;
        m_movementHandler.SetSpeed((float)m_moveSpeed);
    }

    public void SetMoveDirection(Vector3 direction) => m_direction = direction;

    public void SetMoving(bool isMobing) => m_isMove = isMobing;

    public void Initialized(Vector3 dir) => m_initDir = dir;

    private void FixedUpdate()
    {
        m_movementHandler.MoveAll(m_direction);

        //“–‚½‚è”»’è
        float dis = Vector3.Distance(gameObject.transform.position, m_playerPos.Value);

        if(dis < m_collider.radius)
        {
            var fuel = Mathf.Max(0f, m_fuel.Value - m_attack);//ƒ_ƒ[ƒW
            m_fuel.SetValue(fuel);
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }

        if (gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }
    }
}


