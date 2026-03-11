using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget, IDamage
{
    [SerializeField] private EnemyStateMachine m_enemyStateMachine;
    [SerializeField] private EnemySO m_enemySO;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private HealthContainer m_playerHealth;

    public int objectId;
    private SphereCollider m_collider;
    private int m_attack;
    private int m_hp;

    public EnemyStateMachine EnemyStateMachine => m_enemyStateMachine;
    public int ObjectID => objectId;
    public int Attack => m_attack;
    public int HP => m_hp;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public DropSO GetDropData() => m_dropSO;
    [SerializeField] private Vector3 m_reticleOffset;

    public Vector3 ReticleOffset => m_reticleOffset;

    private void Awake()
    {
        m_collider = gameObject.GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        m_attack = m_enemySO.Attack;
        m_hp = m_enemySO.HP;
    }

    private void Update()
    {
        //“–‚½‚è”»’è
        float dis = Vector3.Distance(gameObject.transform.position, m_playerPos.Value);

        if (dis < m_collider.radius)
        {
            m_playerHealth.Damage(m_attack);
            ReturnToPool();
        }

        if(gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            ReturnToPool();
        }
    }

    public void Damage(int damage)
    {
        m_hp -= damage;
        if(m_hp < 0)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        m_enemyStateMachine.ReturnToPool();
    }

    public DropSO Collect()
    {
        DropSO drop = GetDropData();
        ReturnToPool();
        return drop;
    }

    public bool TryCollect(int damage)
    {
        return m_hp - damage <= 0;
    }
}
