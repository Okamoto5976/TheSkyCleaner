using System.Collections;
using UnityEngine;


public class TrashManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private ObjectPoolManager m_pool; // Inspector で割り当て

    [SerializeField] private Vector3 m_spawnPos;            // 生成位置（Zのみ使用）ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMin;       // 最小生成範囲　ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMax;       // 最大生成範囲  ゴミのみ
    [SerializeField] private float m_spawnTrashInterval = 0.3f;

    private WaitForSeconds m_sleepTime;

    [SerializeField] private GameObject m_boss;//のちにコンテナの座標をとる

    [System.Serializable]
    public struct CollectType
    {
        [SerializeField] private ObjectPoolManager m_visualPool;
        [SerializeField] private CollectSO m_collectData;
        public ObjectPoolManager VisualPool => m_visualPool;
        public CollectSO CollectData => m_collectData;
    };

    [Header("Collect Types (Sequences)")]
    [SerializeField] private CollectType[] m_collectTypes;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private Vector3 m_direction;


    private void Awake()
    {
        m_sleepTime = new(m_spawnTrashInterval);

        //StartCoroutine(SpawnOnTimer());
    }

    private IEnumerator SpawnOnTimer()
    {
        while (true)
        {
            yield return m_sleepTime;
            SpawnOne();
        }
    }

    public void SpawnOne()
    {
        if (m_pool == null)
        {
            Debug.LogWarning("[TrashManager] ObjectPoolManager の参照がありません。Inspectorで設定してください。");
            return;
        }

        GameObject obj = m_pool.GetObjectFromPool(); //呼び出し
        var Obj = obj.GetComponent<TrashController>();

        Vector3 randomDir = Random.onUnitSphere;//球体の表面上に点を返す
        randomDir.Normalize();

        Obj.SetMoveDirection(randomDir);
        Obj.SetMoveSpeed(m_moveSpeed);
        Obj.Initialized(m_direction);

        int idx = Random.Range(0, m_collectTypes.Length);
        var seq = m_collectTypes[idx];

        var pool = seq.VisualPool;
        Obj.SetVisual(pool);

        var data = seq.CollectData;
        Obj.SetStatsData(data);
        //ゴミの設定
        SetTrashInfo(obj);

        //Debug.Log(obj);

        return;
    }

    public GameObject SetEnemyThrow()//EnemyがTrash取得につかう
    {
        GameObject obj = m_pool.GetObjectFromPool(); //呼び出し
        var Obj = obj.GetComponent<TrashController>();

        Vector3 randomDir = Random.onUnitSphere;//球体の表面上に点を返す
        randomDir.Normalize();

        Obj.SetMoveDirection(randomDir);
        Obj.SetMoveSpeed(m_moveSpeed);
        Obj.Initialized(m_direction);

        int idx = Random.Range(0, m_collectTypes.Length);
        var seq = m_collectTypes[idx];

        var pool = seq.VisualPool;
        Obj.SetVisual(pool);

        var data = seq.CollectData;
        Obj.SetStatsData(data);

        return obj;
    }

    public GameObject SetThrow(int index)//LargeTrashのほうで呼ぶ
    {
        GameObject obj = m_pool.GetObjectFromPool(); //呼び出し
        var Obj = obj.GetComponent<TrashController>();

        Vector3 randomDir = Random.onUnitSphere;//球体の表面上に点を返す
        randomDir.Normalize();

        Obj.SetMoveDirection(randomDir);
        Obj.SetMoveSpeed(m_moveSpeed);
        Obj.Initialized(m_direction);

        int idx = index;
        var seq = m_collectTypes[idx];

        var pool = seq.VisualPool;
        Obj.SetVisual(pool);

        var data = seq.CollectData;
        Obj.SetStatsData(data);

        return obj;
    }

    public void SetTrashInfo(GameObject obj)
    {
        //SetRandomPosition(obj);
        SetSpawn(obj);
    }
 
    private void SetSpawn(GameObject obj)
    {
        obj.transform.position = m_boss.transform.position;
        obj.SetActive(true);
    }

    private void SetRandomPosition(GameObject obj)
    {
        float randX = UnityEngine.Random.Range(m_spawnTrashMin.x, m_spawnTrashMax.x);
        float randY = UnityEngine.Random.Range(m_spawnTrashMin.y, m_spawnTrashMax.y);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
        obj.SetActive(true);
    }

    public void StartSpawn() { StartCoroutine(SpawnOnTimer()); }
    public void StopSpawn() { StopCoroutine(SpawnOnTimer()); }
}
