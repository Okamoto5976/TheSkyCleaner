//using UnityEngine;

//[CreateAssetMenu(fileName = "ES_TrashSpawnAtEnemy", menuName = "Enemy/States/Trash Spawn At Enemy")]
//public class ES_TrashSpawnAtEnemy : EnemyState
//{
//    [SerializeField] private Transform m_enemyObj;

//    [Header("Movement Settings")]
//    [SerializeField, Min(0.01f)] private float m_trashSpeed = 5f;   // ゴミの移動速度
//    [SerializeField, Min(0.1f)] private float m_lifeTime = 5f;   // プールに戻すまでの寿命(秒)


//    public override void OnEnter()
//    {
//        if (est.PoolObj == null)
//        {
//            Debug.LogWarning("[ES_TrashSpawnAtEnemy] ObjectPoolManager の参照がありません。Inspectorで設定して下さい。");
//            return;
//        }
//    }
//    public override void OnUpdate(float deltaTime)
//    {

//        GameObject trash = est.PoolObj.GetObjectFromPool();

//        // 初期位置
//        SetTrashPosition(trash);

//        Vector3 targetPos = DirToTarget(est.Target.position, trash.transform.position);


//        //移動する向き
//        est.SetMoveDirection(targetPos);

//        trash.SetActive(true);

//    }



//    private void SetTrashPosition(GameObject obj)
//    {
//        obj.transform.position = m_enemyObj.position;
//    }


//}

using UnityEngine;

[CreateAssetMenu(fileName = "ES_TrashSpawnAtEnemy", menuName = "Enemy/States/Trash Spawn At Enemy")]
public class ES_TrashSpawnAtEnemy : EnemyState
{
    [SerializeField] private Transform m_enemyObj;           

    [Header("Movement Settings")]
    [SerializeField, Min(0.01f)] private float m_trashSpeed = 5f;
    [SerializeField, Min(0.1f)] private float m_lifeTime = 5f;   

    private bool _spawnedThisFrame = false; 

    public override void OnEnter()
    {
        if (est == null || est.PoolObj == null)
        {
            Debug.LogWarning("[ES_TrashSpawnAtEnemy] ObjectPoolManager の参照がありません。EnemyStateMachine の PoolObj を設定してください。");
            return;
        }
        _spawnedThisFrame = false;
    }

    public override void OnUpdate(float deltaTime)
    {

        if (_spawnedThisFrame) return;
        _spawnedThisFrame = true;

        // プールから1つ取り出す
        GameObject trash = est.PoolObj.GetObjectFromPool();
        if (trash == null)
        {
            Debug.LogWarning("[ES_TrashSpawnAtEnemy] プールからオブジェクトを取得できませんでした。");
            return;
        }

        // スポーン位置（m_enemyObj が未設定ならこの敵自身の Transform）
        Transform spawnTf = (m_enemyObj != null) ? m_enemyObj : _transform;
        

        // ターゲットのスナップショット（この瞬間だけ使用）
        Transform targetTf = GetTarget();
        Vector3 targetPos = (targetTf != null) ? targetTf.position : (spawnTf.position + _transform.forward);

        // 初期位置・向き
        trash.transform.position = spawnTf.position;

        Vector3 dir = targetPos - spawnTf.position;
        if (dir.sqrMagnitude > 1e-6f) dir.Normalize();
        else dir = _transform.forward;


        // 有効化
        trash.SetActive(true);

        var mover = trash.GetComponent<TrashStraightMover>();
        if (mover == null) mover = trash.AddComponent<TrashStraightMover>();
        mover.Launch(dir, m_trashSpeed, m_lifeTime);
    }

    private sealed class TrashStraightMover : MonoBehaviour
    {
        private Vector3 _dir = Vector3.forward;
        private float _speed = 5f;
        private float _life = 5f;
        private float _t;
        private ReturnObjectToPool _returner;

        public void Launch(Vector3 dir, float speed, float life)
        {
            _dir = dir.sqrMagnitude > 0f ? dir.normalized : Vector3.forward;
            _speed = Mathf.Max(0f, speed);
            _life = Mathf.Max(0.01f, life);
            _t = 0f;

            if (_returner == null) _returner = GetComponent<ReturnObjectToPool>();
            enabled = true;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            transform.position += _dir * _speed * dt;
            _t += dt;

            if (_t >= _life)
            {
                if (_returner != null) _returner.ReturnToPool();
                else gameObject.SetActive(false);
            }
        }
    }
}