using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵1体ごとにアタッチ。指定の EnemySequence を順番に処理する。
/// </summary>
[DisallowMultipleComponent]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_target;
    [SerializeField] private ObjectPoolManager m_pool; // プール（投擲ゴミなどで使用）
    [SerializeField] private Logger m_logger;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeed = 5f;
    [Tooltip("MovementHandler の MoveAll(Vector3) に渡す値が『方向』なら true。『速度(=方向×速度)』を受け取る設計なら false にするなど、適宜調整。")]
    [SerializeField] private bool m_moveAllTakesDirection = true;

    [Header("Sequence")]
    [SerializeField] private EnemySequence m_sequence;
    [SerializeField] private bool m_loopSequence = false;

    private int m_index = 0;
    private EnemyState m_current;
    private MovementHandler m_movementHandler; 
    private bool m_running;

    public Transform Target => m_target;
    public ObjectPoolManager Pool => m_pool;
    public Logger Log => m_logger;
    public float MoveSpeed
    {
        get => m_moveSpeed;
        set => m_moveSpeed = value;
    }

    public void MoveDirection(Vector3 worldDirection)
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
        var status = m_current.OnUpdate(this, Time.deltaTime);
        if (status == StateStatus.Success || status == StateStatus.Failure)
        {
            // 終了処理
            m_current.OnExit(this);

            // 次へ
            m_index++;

            // ループ処理
            if (m_sequence == null || m_sequence.States.Count == 0)
            {
                m_running = false;
                return;
            }

            if (m_index >= m_sequence.States.Count)
            {
                if (m_loopSequence)
                {
                    m_index = 0;
                }
                else
                {
                    m_running = false;
                    return;
                }
            }

            // 次のステートへ
            m_current = m_sequence.States[m_index].State;
            m_current.OnEnter(this);
        }
    }

    // ====== 公開操作 ======

    public void SetTarget(Transform t) => m_target = t;
    public void SetPool(ObjectPoolManager p) => m_pool = p;
    public void SetLogger(Logger logger) => m_logger = logger;

    public void SetSequence(EnemySequence sequence, bool loop = false)
    {
        m_sequence = sequence;
        m_loopSequence = loop;
        ResetMachine();
        if (isActiveAndEnabled) StartMachine();
    }

    public void StartMachine()
    {
        if (m_sequence == null || m_sequence.States.Count == 0) return;
        m_index = 0;
        m_current = m_sequence.States[m_index].State;
        m_current.OnEnter(this);
        m_running = true;
    }

    public void ResetMachine()
    {
        StopAllCoroutines();
        m_index = 0;
        m_current = null;
        m_running = false;
    }
}