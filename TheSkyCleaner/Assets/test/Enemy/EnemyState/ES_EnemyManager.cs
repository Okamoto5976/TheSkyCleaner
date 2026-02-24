using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プールから敵オブジェクトを取得し、EnemySequence（敵1/2/3）をランダムに割り当てる。
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private EnemyPoolManager m_poolEnemy;
    [SerializeField] private TrashPoolManager m_poolTrash;
    [SerializeField] private TrashManager m_trashManager;

    [SerializeField] public AxisVector3Container m_target;

    [Header("Spawn Area (X,Y random / Z fixed)")]
    [SerializeField] private Vector3 m_spawnPos;      // z のみ使用想定
    [SerializeField] private Vector2 m_spawnMin;      // X,Y min
    [SerializeField] private Vector2 m_spawnMax;      // X,Y max
    [SerializeField] private float m_spawnInterval = 0.3f;

    [System.Serializable]
    public struct EnemyTypes
    {
        [SerializeField] private EnemySequence m_enemyType;
        [SerializeField] private ObjectPoolManager m_visualPool;
        [SerializeField] private EnemySO m_enemyData;
        public EnemySequence EnemyType => m_enemyType;
        public ObjectPoolManager VisualPool => m_visualPool;
        public EnemySO EnemyData => m_enemyData;
    };
    [Header("Enemy Types (Sequences)")]
    [SerializeField] private EnemyTypes[] m_enemyTypes;
    

    [Header("Default Movement")]
    [SerializeField] private bool m_loopSequence = false;

    private WaitForSeconds m_wait;


    private void Awake()
    {
        m_wait = new WaitForSeconds(m_spawnInterval);
        //StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return m_wait;
            SpawnOne();
        }
    }

    public void SpawnOne()
    {
        if (m_poolEnemy == null)
        {
            Debug.LogWarning("[EnemyManager] ObjectPoolManager の参照がありません。Inspector で設定してください。");
            return;
        }

        if (m_enemyTypes == null || m_enemyTypes.Length == 0)
        {
            Debug.LogWarning("[EnemyManager] EnemySequence が設定されていません。");
            return;
        }

        T_Enemy obj = m_poolEnemy.GetComponentFromPool();
        SetEnemyInfo(obj);
    }

    private void SetEnemyInfo(T_Enemy obj)
    {
        SetRandomPosition(obj.gameObject);
        var machine = obj.EnemyStateMachine;

        // 敵1/2/3 をランダム選択
        int idx = Random.Range(0, m_enemyTypes.Length);
        var seq = m_enemyTypes[idx];

        var pool = seq.VisualPool;
        obj.SetVisual(pool);

        var data = seq.EnemyData;
        obj.SetStatsData(data);

        List<EnemyStateMachine.StateMachineInstance> seqInstance = new();
        foreach (var s in seq.EnemyType.States)
        {
            EnemyStateMachine.StateMachineInstance newState = new()
            {
                state = Instantiate(s.state),
                time = s.time,
                isActive = false,
            };

            newState.state.InjectVariables(machine);

            seqInstance.Add(newState);
        }



        // 参照注入
        machine.SetTarget(m_target);
        machine.SetPool(m_poolEnemy);
        machine.SetPoolObj(m_poolTrash);
        machine.SetTrashManager(m_trashManager);
        machine.SetLogger(m_logger);
       


        // シーケンスを割り当てて開始
        machine.SetSequence(seqInstance, m_loopSequence);

        machine.Initialize();
    }

    private void SetRandomPosition(GameObject obj)
    {
        // 位置ランダム
        float randX = Random.Range(m_spawnMin.x, m_spawnMax.x);
        float randY = Random.Range(m_spawnMin.y, m_spawnMax.y);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    }

    public void StartSpawn() { StartCoroutine(SpawnLoop()); }
    public void StopSpawn() { StopCoroutine(SpawnLoop()); }
}