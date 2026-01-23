using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class TrashManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private ObjectPoolManager m_pool; // Inspector で割り当て

    [SerializeField] private Vector3 m_spawnPos;            // 生成位置（Zのみ使用）ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMin;       // 最小生成範囲　ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMax;       // 最大生成範囲  ゴミのみ
    [SerializeField] private float m_spawnTrashInterval = 0.3f;

    private WaitForSeconds m_sleepTime;

    private void Awake()
    {
        m_sleepTime = new(m_spawnTrashInterval);
        StartCoroutine(SpawnOnTimer());
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
            Debug.LogWarning("[SpawnTEManager] ObjectPoolManager の参照がありません。Inspectorで設定してください。");
            return;
        }

        GameObject obj = m_pool.GetFromPool(true); //呼び出し
            //ゴミの設定
            SetTrashInfo(obj);

        //Debug.Log(obj);

        return;
    }

    public void SetTrashInfo(GameObject obj)
    {
        SetRandomPosition(obj);
        //m_logger.Log(obj.name + ":ゴミです", this);
    }
 
    private void SetRandomPosition(GameObject obj)
    {
        float randX = UnityEngine.Random.Range(m_spawnTrashMin.x, m_spawnTrashMax.x);
        float randY = UnityEngine.Random.Range(m_spawnTrashMin.y, m_spawnTrashMax.y);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    }
}
