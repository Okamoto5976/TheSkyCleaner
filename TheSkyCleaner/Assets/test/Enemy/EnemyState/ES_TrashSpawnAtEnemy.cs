using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ES_TrashSpawnAtEnemy", menuName = "Enemy/States/Trash Spawn At Enemy")]
public class ES_TrashSpawnAtEnemy : EnemyState
{
    [SerializeField] public AxisVector3Container m_targetPos;
    [SerializeField] private float m_trashSpeed = 20f;
  
    
    private GameObject m_trash; //ゴミ本体
    private bool m_isSpawnTrash = true;

    public override void OnEnter()
    {
        //OnEnterがちゃんと呼べようにしたい。
        //理由は、敵の行動が次のstateに移行したときに、
        Debug.Log("OnEnter");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnter");
        m_isSpawnTrash = true;
    }

    public override void OnUpdate(float deltaTime)
    {
        var pool = est.PoolObj as ObjectPoolManager;

        // 一定間隔で取得して初期化
        if(m_isSpawnTrash)
        {
            m_isSpawnTrash=false;

            //取得
            m_trash = pool.GetObjectFromPool();
            m_trash.GetComponent<MovementHandler>().enabled = false;
            m_trash.GetComponent<ConstantFloatEvent>().enabled = false;

            //生成位置 
            m_trash.transform.position = _transform.position;

            m_trash.SetActive(true);
        }
        if (m_trash == null) return;

        //target取得
        Vector3 dir = DirToTarget(m_targetPos.Value, m_trash.transform.position);
        //m_trash.GetComponent<MovementHandler>().SetSpeed(m_trashSpeed);
        //m_trash.GetComponent<MovementHandler>().MoveAll(dir);
       // m_trash.GetComponent<ConstantFloatEvent>().m_onMove

        m_trash.transform.position += dir * m_trashSpeed * deltaTime;


        // Z が重なったらpoolに戻す
        if (Mathf.Abs(m_trash.transform.position.z - m_targetPos.Value.z) < 0.01f)
        {
            m_trash.GetComponent<ReturnObjectToPool>().ReturnToPool();
            m_trash.GetComponent<MovementHandler>().enabled =    true;
            m_trash.GetComponent<ConstantFloatEvent>().enabled = true;

            m_trash = null;
        }
    }

}