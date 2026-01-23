
using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnTEManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private EnemyPoolManager m_pool; // Inspector で割り当て


    [SerializeField] private Vector3 m_spawnPos;            // 生成位置（Zのみ使用）ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMin;       // 最小生成範囲　ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMax;       // 最大生成範囲  ゴミのみ
    [SerializeField] private float m_spawnTrashInterval = 0.3f;

    private bool m_isTrash = true;  //ゴミかどうかの判別
    private WaitForSeconds m_sleepTime;


    private void Awake()
    {
        m_sleepTime = new(m_spawnTrashInterval);
        StartCoroutine(SpawnOnTimer());
    }

    private IEnumerator SpawnOnTimer()
    {
        while(true)
        {
            yield return m_sleepTime;
            SpawnOne();
        }
    }

    public void SpawnOne()
    {
        m_logger.Log($"Test", this);
        if (m_pool == null)
        {
            Debug.LogWarning("[SpawnTEManager] ObjectPoolManager の参照がありません。Inspectorで設定してください。");
            return;
        }

        var obj = m_pool.GetEnemyFromPool(); //呼び出し
        int random = UnityEngine.Random.Range(1, 10);
        switch(random)
        {
            case 1: case 2: case 3: case 4:
                m_isTrash=true; break;
            case 5: default:
                m_isTrash=false; break;
        };
        if (m_isTrash)
        {
            //ゴミの設定
            SetTrashInfo(obj.gameObject);
        }
        else
        {
            //敵の設定
            SetEnemyInfo(obj.gameObject);
        }
        obj.gameObject.SetActive(true);
        return;
    }

    public void SetTrashInfo(GameObject obj)
    {
        // X/Y を指定範囲でランダム、Z は既存の m_spawnPos.z を採用
        SetRandomPosition(obj);
        m_logger.Log($"{obj.name}:ゴミです", this);
        //m_pool.ReturnToPool(m_poolObj);

    }
    public void SetEnemyInfo(GameObject obj)
    {
        SetRandomPosition(obj);
        m_logger.Log($"{obj.name}:敵です", this);
    }

    private void SetRandomPosition(GameObject obj)
    {
        float randX = UnityEngine.Random.Range(m_spawnTrashMin.x, m_spawnTrashMax.x);
        float randY = UnityEngine.Random.Range(m_spawnTrashMin.y, m_spawnTrashMax.y);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    }
}
