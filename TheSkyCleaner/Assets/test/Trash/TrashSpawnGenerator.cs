
using System.Collections;
using UnityEngine;
using System.Reflection; // ★ 追加：リフレクション用

public class TrashSpawnGenerator : MonoBehaviour
{
    [Header("生成するプレハブ/基準Z")]
    [SerializeField] private GameObject m_spawnObj;   // オブジェクト
    [SerializeField] private Vector3 m_spawnPos;      // 生成位置（Zのみ使用）

    [Header("X/Y のランダム生成範囲")]
    [SerializeField] private float m_spawnXMin;       // X 最小
    [SerializeField] private float m_spawnXMax;       // X 最大
    [SerializeField] private float m_spawnYMin;       // Y 最小
    [SerializeField] private float m_spawnYMax;       // Y 最大

    [Header("生成パラメータ")]
    [SerializeField] private int m_spawnObj_Max_Count; // 最大生成数
    [SerializeField] private float m_spawnInterval;    // 生成間隔

    [Tooltip("TrashMoverに渡す値")]
    [Header("TrashMoverの設定")]
    //[SerializeField] private Vector3 m_targetPosition;
    [SerializeField] private Vector3 m_trashMoveSpeed;     // ★ ここを Inspector から調整
    [SerializeField] private int m_DestroyPosZ;     // ★ ここを Inspector から調整

    private void Start()
    {
        StartCoroutine("TrashCount");
    }


    //public void 
    private IEnumerator TrashCount()
    {
        for (int count = 0; count < m_spawnObj_Max_Count; count++)
        {
            yield return new WaitForSeconds(m_spawnInterval);

            // X/Y を指定範囲でランダム、Z は既存の m_spawnPos.z を採用
            float randX = Random.Range(m_spawnXMin, m_spawnXMax);
            float randY = Random.Range(m_spawnYMin, m_spawnYMax);
            Vector3 spawn = new Vector3(randX, randY, m_spawnPos.z);

            GameObject obj = Instantiate(m_spawnObj, spawn, Quaternion.identity);

            // ★ TrashMoverのm_trashMoveSpeedにTrashSpawnGeneratorの設定を渡す
            //var mover = obj.GetComponent<TrashMover>(); //Scripte名
            //if (mover != null)
            //{
            //   mover.m_trashMoveSpeed = m_trashMoveSpeed;
            //   mover.m_DestroyPosZ = m_DestroyPosZ;
            //}
            //else
            //{
            //    Debug.LogWarning("[TrashSpawnManager] 生成したオブジェクトに TrashMover が見つかりません。プレハブにアタッチされているか確認してください。");
            //}
        }
    }
}
