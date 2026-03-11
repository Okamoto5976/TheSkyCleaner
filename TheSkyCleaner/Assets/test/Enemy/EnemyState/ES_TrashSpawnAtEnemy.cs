using UnityEngine;

[CreateAssetMenu(fileName = "ES_TrashSpawnAtEnemy", menuName = "Enemy/States/Trash Spawn At Enemy")]
public class ES_TrashSpawnAtEnemy : EnemyState
{
    [SerializeField] public AxisVector3Container m_targetPos;
    [SerializeField] private float m_trashSpeed = 20f;
  
    
    private TrashController m_trash; //ƒSƒ~–{‘Ì
    private bool m_isSpawnTrash = true;

    public override void OnEnter()
    {
        m_isSpawnTrash = true;
    }

    public override void OnUpdate(float deltaTime)
    {
        var pool = est.PoolTrash;

        // ˆê’èŠÔŠu‚Åæ“¾‚µ‚Ä‰Šú‰»
        if (m_isSpawnTrash)
        {
            m_isSpawnTrash = false;

            //æ“¾
            m_trash = pool.GetComponentFromPool();


            //¶¬ˆÊ’u 
            m_trash.transform.position = _transform.position;

            m_trash.gameObject.SetActive(true);

        }
        if (m_trash == null) return;

        m_trash.SetMoving(true);
        m_trash.SetMoveSpeed(m_trashSpeed);
        Vector3 dir = DirToTarget(m_targetPos.Value, _transform.position);
        m_trash.SetMoveDirection(dir);
    }

    public override void OnExit()
    {
        m_isSpawnTrash = false;
    }

}