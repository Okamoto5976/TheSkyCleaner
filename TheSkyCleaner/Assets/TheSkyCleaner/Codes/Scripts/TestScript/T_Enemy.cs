using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget, IDamage
{
    [SerializeField] private EnemyStateMachine m_enemyStateMachine;
    [SerializeField] private EnemySO m_enemySO;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private FloatContainer m_fuel;

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
            var fuel = Mathf.Max(0f,m_fuel.Value - m_attack);//ƒ_ƒ[ƒW
            m_fuel.SetValue(fuel);
            m_enemyStateMachine.ReturnToPool();
        }

        if(gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            m_enemyStateMachine.ReturnToPool();
        }
    }

    public void Damage(int damage)
    {
        m_hp -= damage;
        if(m_hp < 0)
        {
            m_enemyStateMachine.ReturnToPool();
        }
    }
}
