using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class TrashController : MonoBehaviour, ILockOnTarget
{
    private CollectSO m_collectSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private HealthContainer m_playerHealth;

    [SerializeField] private IntegerContainer m_scoreContainer;
    [SerializeField] private int m_score;

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

    [SerializeField] private Vector3 m_reticleOffset;

    public Vector3 ReticleOffset => m_reticleOffset;
    //visualに関わるもの
    [SerializeField] private Transform m_root;

    private ReturnObjectToPool m_visualreturn;

    //ランダムな方向に動く後、時間経過でz軸に動く
    private float m_moveTime = 2f;

    private void Awake()
    {
        m_transform = transform;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveTime());
    }

    private IEnumerator MoveTime()
    {
        yield return new WaitForSeconds(m_moveTime);

        SetMoveDirection(m_initDir);
        yield break;
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

        //当たり判定
        float dis = Vector3.Distance(gameObject.transform.position, m_playerPos.Value);

        
        if (gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            ReturnToPool();
        }

        if (m_collider == null) return;

        if (dis < m_collider.radius)
        {
            m_playerHealth.Damage(m_attack);
            ReturnToPool();
        }

    }

    public void SetVisual(ObjectPoolManager pool)
    {
        //return処理
        if (m_visualreturn != null)
        {
            m_visualreturn.ReturnToPool();
            m_visualreturn = null;
        }

        //visual適応
        GameObject visual = pool.GetObjectFromPool();

        visual.transform.SetParent(m_root);
        visual.transform.localPosition = Vector3.zero;
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale = Vector3.one;

        visual.SetActive(true);

        m_visualreturn = visual.GetComponent<ReturnObjectToPool>();

        m_collider = visual.GetComponent<SphereCollider>();
    }

    public void SetStatsData(CollectSO collectSO)
    {
        m_collectSO = collectSO;
        m_attack = m_collectSO.Attack;
        m_hp = m_collectSO.HP;
    }

    private void ReturnToPool()
    {
        m_returnObjectToPool.ReturnToPool();
        m_movementHandler.MoveAll(m_initDir);
    }

    public DropSO Collect()
    {
        DropSO drop = GetDropData();
        AddScore(m_score);
        ReturnToPool();
        return drop;
    }

    public void AddScore(int value)
    {
        int score = m_scoreContainer.Value + value;
        m_scoreContainer.SetValue(score);
    }
}


