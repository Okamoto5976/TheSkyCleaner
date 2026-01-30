using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class EnemtManager : MonoBehaviour
{
    [SerializeField] private Logger m_logger;
    [SerializeField] private ObjectPoolManager m_pool; // Inspector で割り当て

    [SerializeField] private Transform m_target;


    [SerializeField] private Vector3 m_spawnPos;            // 生成位置（Zのみ使用）ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMin;       // 最小生成範囲　ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMax;       // 最大生成範囲  ゴミのみ
    [SerializeField] private float m_spawnTrashInterval = 0.3f;

    [Header("Enemy")]
    [SerializeField] private Vector3 m_enemySpeed;

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

        GameObject obj = m_pool.GetObjectFromPool(); //呼び出し
            SetEnemyInfo(obj);

        //Debug.Log(obj);

        return;
    }

    public void SetEnemyInfo(GameObject obj)
    {
        SetRandomPosition(obj);
        //m_logger.Log(obj.name + ":敵です", this);

        int random = UnityEngine.Random.Range(0, 1);
        switch (random)
        {
            case 0:
                EnemyController(obj); break;
            case 1: break;
            case 2: break;

        }
        ;
    }

    public void EnemyController(GameObject obj)
    {
        //SetRandomPosition(obj);

        //obj.GetComponent<MovementHandler>().enabled = false;
        //obj.GetComponent<ConstantFloatEvent>().enabled = false;
        //obj.GetComponent<ReturnObjectToPool>().enabled = false;

        if (m_target == null)
        {
            Debug.LogError("targetにオブジェクトが入っていません");
            //return;
        }

        // オブジェクトごとに位相を少し変えて動きをバラけさせる

        // まっすぐターゲットへ移動（等速）。必要最低限だけ。
        IEnumerator FlyStraight()
        {
            // 到達判定のしきい値（必要なら調整）
            const float arriveDistance = 0.5f;

            while (obj != null && obj.activeInHierarchy && m_target != null)
            {
                Vector3 toTarget = m_target.position - obj.transform.position;
                float sqr = toTarget.sqrMagnitude;
                if (sqr < arriveDistance * arriveDistance) break;

                Vector3 dir = toTarget.normalized;

                // 等速で前進
                obj.transform.position += dir * m_enemySpeed.z * Time.deltaTime;
                //obj.transform.position = new Vector3(m_moveMentHandler.MoveAll(dir * m_enemySpeed.z * Time.deltaTime));

                // 向きを合わせる（任意だが見た目のため）
                obj.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                yield return null;
            }

            // 任意：到達後にプールへ返却（ReturnObjectToPoolは無効化しているためここで返す）
            if (obj != null && obj.activeInHierarchy && m_pool != null)
            {
                m_pool.GetObjectFromPool();
            }
        }

        StartCoroutine(FlyStraight());
    }


    private void SetRandomPosition(GameObject obj)
    {
        float randX = UnityEngine.Random.Range(m_spawnTrashMin.x, m_spawnTrashMax.x);
        float randY = UnityEngine.Random.Range(m_spawnTrashMin.y, m_spawnTrashMax.y);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    }
}
