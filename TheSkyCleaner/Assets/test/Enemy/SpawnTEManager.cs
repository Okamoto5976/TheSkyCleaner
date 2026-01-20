
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class SpawnTEManager : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager m_pool; // Inspector で割り当て


    [SerializeField] private Vector3 m_spawnPos;            // 生成位置（Zのみ使用）ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMin;       // 最小生成範囲　ゴミのみ
    [SerializeField] private Vector2 m_spawnTrashMax;       // 最大生成範囲  ゴミのみ
    [SerializeField] private float m_spawnTrashInterval = 0.3f;

    private bool m_isTrash = true;  //ゴミかどうかの判別

    private GameObject m_poolObj;

    public GameObject SpawnOne()
    {
        if (m_pool == null)
        {
            Debug.LogWarning("[SpawnTEManager] ObjectPoolManager の参照がありません。Inspectorで設定してください。");
            return null;
        }

        m_poolObj = m_pool.GetFromPool(true); //呼び出し
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
            SetTrashInfo();
        }
        else
        {
            //敵の設定
            SetEnemyInfo();
        }

        Debug.Log(m_poolObj);
        return m_poolObj;
    }

    //public void SetTrashInfo()
    //{
    //        // X/Y を指定範囲でランダム、Z は既存の m_spawnPos.z を採用
    //        float randX = UnityEngine.Random.Range(m_spawnTrashMin.x, m_spawnTrashMax.x);
    //        float randY = UnityEngine.Random.Range(m_spawnTrashMin.y, m_spawnTrashMax.y);
    //        m_poolObj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    //        Debug.Log(m_poolObj.name + ":ゴミです");
    //        //m_pool.ReturnToPool(m_poolObj);
    //}

    public IEnumerator SetTrashInfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_spawnTrashInterval);
            // X/Y を指定範囲でランダム、Z は既存の m_spawnPos.z を採用
            float randX = UnityEngine.Random.Range(m_spawnTrashMin.x, m_spawnTrashMax.x);
            float randY = UnityEngine.Random.Range(m_spawnTrashMin.y, m_spawnTrashMax.y);
            m_poolObj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
            Debug.Log(m_poolObj.name + ":ゴミです");
            //m_pool.ReturnToPool(m_poolObj);
        }
    }
    public void SetEnemyInfo()
    {
        Debug.Log(m_poolObj.name + ":敵です");
    }
}
