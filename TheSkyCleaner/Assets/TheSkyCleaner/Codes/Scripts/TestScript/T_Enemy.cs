using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget, IDamage
{
    [SerializeField] private EnemyStateMachine m_enemyStateMachine;
    private EnemySO m_enemySO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private HealthContainer m_playerHealth;
    [SerializeField] private AudioContainer m_deathSound;


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
    public DropSO GetDropData() => m_enemySO.Drop;

    [SerializeField] private Vector3 m_reticleOffset;
    public Vector3 ReticleOffset => m_reticleOffset;
    //visual‚ÉŠÖ‚í‚é‚à‚Ì
    [SerializeField] private Transform m_root;

    private ReturnObjectToPool m_visualreturn;
    private ObjectPoolManager m_poolDeathParticle;
    private AudioSource m_audioSource;

    public void SetPoolDeathEffect(ObjectPoolManager t) => m_poolDeathParticle = t;
    public void SetAudioSource(AudioSource audioSource) => m_audioSource = audioSource;

    private void Awake()
    {
        m_collider = gameObject.GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {

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
        if(m_hp <= 0)
        {
            GameObject deathParticle = m_poolDeathParticle.GetObjectFromPool();
            deathParticle.transform.position = Transform.position;
            deathParticle.SetActive(true);
            m_audioSource.PlayOneShot(m_deathSound.AudioClip, m_deathSound.Volume);
            ReturnToPool();
        }
    }

    public void SetVisual(ObjectPoolManager pool)
    {
        //returnˆ—
        if(m_visualreturn != null)
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

    public void SetStatsData(EnemySO enemySO)
    {
        m_enemySO = enemySO;
        m_attack = m_enemySO.Attack;
        m_hp = m_enemySO.HP;
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
        m_hp -= damage;
        return m_hp - damage <= 0;
    }
}
