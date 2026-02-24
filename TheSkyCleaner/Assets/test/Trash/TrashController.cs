using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class TrashController : MonoBehaviour, ILockOnTarget, IDamage
{
    private CollectSO m_collectSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private HealthContainer m_playerHealth;

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
    private int m_hp;
    public Transform Transform => m_transform;

    public GameObject GameObject => gameObject;

    public DropSO GetDropData()=> m_collectSO.Drop;

    //visual‚ÉŠÖ‚í‚é‚à‚Ì
    [SerializeField] private Transform m_root;

    private ReturnObjectToPool m_visualreturn;

    private void Awake()
    {
        m_collider = gameObject.GetComponent<SphereCollider>();
        m_transform = transform;
    }

    private void OnEnable()
    {

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
            m_playerHealth.Damage(m_attack);
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }

        if (gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }
    }

    public void Damage(int damage)
    {
        m_hp -= damage;

        if (m_hp <= 0)
        {

            m_returnObjectToPool.ReturnToPool();
        }
    }

    public void SetVisual(ObjectPoolManager pool)
    {
        //returnˆ—
        if (m_visualreturn != null)
        {
            m_visualreturn.ReturnToPool();
            m_visualreturn = null;
        }

        //visual“K‰ž
        GameObject visual = pool.GetObjectFromPool();

        visual.transform.SetParent(m_root);
        visual.transform.localPosition = Vector3.zero;
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale = Vector3.one;

        visual.SetActive(true);

        m_visualreturn = visual.GetComponent<ReturnObjectToPool>();
    }

    public void SetStatsData(CollectSO collectSO)
    {
        m_collectSO = collectSO;
        m_attack = m_collectSO.Attack;
        m_hp = m_collectSO.HP;
    }
}


