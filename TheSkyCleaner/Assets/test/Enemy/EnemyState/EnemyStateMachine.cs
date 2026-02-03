using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵1体ごとにアタッチ。指定の EnemySequence を順番に処理する。
/// </summary>
[DisallowMultipleComponent]
public class EnemyStateMachine : MonoBehaviour
{
    [System.Serializable]
    public class StateMachineInstance
    {
        public EnemyState state;
        public Vector2 time;
        public bool isActive;
    };

    [Header("References")]
    private Transform m_target;
    private EnemyPoolManager m_pool; // プール（投擲ゴミなどで使用）
    private Logger m_logger;
    [SerializeField]private MovementHandler m_movementHandler;
    [SerializeField] private ReturnObjectToPool m_returnObjectToPool;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeed = 5f;
    [Tooltip("MovementHandler の MoveAll(Vector3) に渡す値が『方向』なら true。『速度(=方向×速度)』を受け取る設計なら false にするなど、適宜調整。")]
    //[SerializeField] private bool m_moveAllTakesDirection = true;


    //[("Sequence")]
    private List<StateMachineInstance> m_sequence;
    private bool m_loopSequence = false;

    private float m_runningTime;

    public Transform Target => m_target;
    public EnemyPoolManager Pool => m_pool;
    public Logger Log => m_logger;

    public MovementHandler MovementHandler => m_movementHandler;
    public float MoveSpeed
    {
        get => m_moveSpeed;
        set => m_moveSpeed = value;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        m_movementHandler.SetSpeed((float)moveSpeed);
    }
    public void SetMoveDirection(Vector3 worldDirection)
    {
        m_movementHandler.MoveAll(worldDirection);
    }


    //private void OnEnable()
    //{
    //    // 再利用（プール）時の安全な初期化
    //    ResetMachine();
    //    if (m_sequence != null) StartMachine();
    //}

    //private void OnDisable()
    //{
    //    StopAllCoroutines();
    //    m_running = false;
    //    m_current = null;
    //}

    private void Update()
    {
        m_runningTime += Time.deltaTime;

        bool hasSequence = m_sequence
            .Select(x => x.time.y > m_runningTime)
            .Any();
        if (!hasSequence)
        {
            //全ての state が終わった時
            //ru-pusurunara reset
            //ru-pushinainara return
            return;
        }

        for (int i = 0; i < m_sequence.Count; i++)
        {
            if (m_runningTime > m_sequence[i].time.x && !m_sequence[i].isActive)
            {
                m_sequence[i].isActive = true;
                // stateが始まるときに呼ぶ関数
            }
            if (m_runningTime > m_sequence[i].time.y && m_sequence[i].isActive)
            {
                m_sequence[i].isActive = false;
                // stateが終わる時に呼ぶ関数
            }
        }


        var sequence = m_sequence
            .Where(x => x.isActive)
            .Select(x => x.state);
        foreach (var state in sequence)
        {
            state.OnUpdate(Time.deltaTime);
        }

    }

    // ====== 公開操作 ======

    public void SetTarget(Transform t) => m_target = t;
    public void SetPool(EnemyPoolManager p) => m_pool = p;
    public void SetLogger(Logger logger) => m_logger = logger;

    public void SetSequence(List<StateMachineInstance> sequence, bool loop = false)
    {
        m_sequence = sequence;
        m_loopSequence = loop;
    }

    public void Initialize()
    {
        ResetMachine();
        StartMachine();
        gameObject.SetActive(true);
    }

    public void StartMachine()
    {
        if (m_sequence == null || m_sequence.Count == 0) return;
    }

    public void ResetMachine()
    {
        StopAllCoroutines();
        m_runningTime = 0;
    }

    public void ReturnToPool()
    {
        m_returnObjectToPool.ReturnToPool();
    }
}