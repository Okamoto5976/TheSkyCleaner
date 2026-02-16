using System.Collections;
using UnityEngine;

public class LargeTrashManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private ObjectPoolManager m_poollargetrash; // Inspector で割り当て
    [SerializeField] private ObjectPoolManager m_pooltrash;

    [SerializeField] private Vector3 m_spawnPos;            // 生成位置（Zのみ使用）ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMin;       // 最小生成範囲　ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMax;       // 最大生成範囲  ゴミのみ
    [SerializeField] private float m_spawnTrashInterval = 0.3f;

    private WaitForSeconds m_sleepTime;

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
        if (m_poollargetrash == null)
        {
            Debug.LogWarning("[TrashManager] ObjectPoolManager の参照がありません。Inspectorで設定してください。");
            return;
        }

        GameObject obj = m_poollargetrash.GetObjectFromPool(); //呼び出し
        var Obj = obj.GetComponent<LargeTrashController>();
        Obj.SetMoveDirection(m_direction);
        Obj.SetMoveSpeed(m_moveSpeed);
        Obj.Initialized(m_direction);
        //Debug.Log($"{m_moveSpeed}");
        //Debug.Log($"{m_direction}");
        //ゴミの設定
        SetTrashInfo(obj);

        //Debug.Log(obj);

        Obj.SetPoolObj(m_pooltrash);//LargeTrashControllerにtrashを呼べるよう渡す


        return;
    }

    public void SetTrashInfo(GameObject obj)
    {
        SetRandomPosition(obj);
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
