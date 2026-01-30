using System.Collections;
using UnityEngine;

/// <summary>
/// プールから敵オブジェクトを取得し、EnemySequence（敵1/2/3）をランダムに割り当てる。
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private ObjectPoolManager m_pool;

    [SerializeField] private Transform m_target;

    [Header("Spawn Area (X,Y random / Z fixed)")]
    [SerializeField] private Vector3 m_spawnPos;      // z のみ使用想定
    [SerializeField] private Vector2 m_spawnMin;      // X,Y min
    [SerializeField] private Vector2 m_spawnMax;      // X,Y max
    [SerializeField] private float m_spawnInterval = 0.3f;

    [Header("Enemy Types (Sequences)")]
    [Tooltip("敵1 / 敵2 / 敵3 用のシーケンスを設定。必要なら数を増やしてOK。")]
    [SerializeField] private EnemySequence[] m_enemyTypes;

    [Header("Default Movement")]
    [SerializeField] private bool m_loopSequence = false;

    private WaitForSeconds m_wait;

    private void Awake()
    {
        m_wait = new WaitForSeconds(m_spawnInterval);
        StartCoroutine(SpawnLoop());
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
        if (m_pool == null)
        {
            Debug.LogWarning("[EnemyManager] ObjectPoolManager の参照がありません。Inspector で設定してください。");
            return;
        }

        if (m_enemyTypes == null || m_enemyTypes.Length == 0)
        {
            Debug.LogWarning("[EnemyManager] EnemySequence が設定されていません。");
            return;
        }

        GameObject obj = m_pool.GetFromPool(true);
        SetEnemyInfo(obj);
    }

    private void SetEnemyInfo(GameObject obj)
    {
        SetRandomPosition(obj);

        // 敵1/2/3 をランダム選択
        int idx = Random.Range(0, m_enemyTypes.Length);
        var seq = m_enemyTypes[idx];

        // ステートマシンを取得（プールのプレハブに付けておく）
        var machine = obj.GetComponent<EnemyStateMachine>();

        // 参照注入
        machine.SetTarget(m_target);
        machine.SetPool(m_pool);
        machine.SetLogger(m_logger);


        // シーケンスを割り当てて開始
        machine.SetSequence(seq, m_loopSequence);

        if (obj != null && obj.activeInHierarchy && m_pool != null)
        {
            m_pool.ReturnToPool(obj);
        }
    }

    private void SetRandomPosition(GameObject obj)
    {
        // 位置ランダム
        float randX = Random.Range(m_spawnMin.x, m_spawnMax.x);
        float randY = Random.Range(m_spawnMin.y, m_spawnMax.y);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    }
}