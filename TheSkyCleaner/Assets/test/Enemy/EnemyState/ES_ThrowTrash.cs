using UnityEngine;

[CreateAssetMenu(fileName = "ES_ThrowTrash", menuName = "Enemy/States/Throw Trash (Non-homing)")]
public class ES_ThrowTrash : EnemyState
{
    [Header("Spawn")]
    [SerializeField] private GameObject trashPrefab;
    [SerializeField] private Vector3 spawnOffset = Vector3.forward * 0.5f;

    [Header("Throwing")]
    [SerializeField] private float throwSpeed = 10f;
    [SerializeField] private bool useRigidbodyVelocity = true;

    [SerializeField] private float autoReturnAfter = 5f; 

    public override StateStatus OnUpdate(EnemyStateMachine est, float deltaTime)
    {
        if (trashPrefab == null) return StateStatus.Failure;

        Transform target = GetTarget(est);
        if (target == null) return StateStatus.Failure;

        
        Vector3 targetPos = target.position;

        
        GameObject trashObj = null;
        if (est.Pool != null)
        {
            trashObj = est.Pool.GetFromPool(true); 
            
            
            
        }
        else
        {
            trashObj = GameObject.Instantiate(trashPrefab);
        }

        
        Vector3 spawnPos = est.transform.position + est.transform.TransformVector(spawnOffset);
        trashObj.transform.position = spawnPos;

        Vector3 dir = (targetPos - spawnPos);
        if (dir.sqrMagnitude > 0.0001f) dir.Normalize(); else dir = est.transform.forward;
        trashObj.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        
        if (useRigidbodyVelocity && trashObj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = dir * throwSpeed;
        }
        else
        {
            
            var p = trashObj.GetComponent<ES_TrashProjectile>();
            if (p == null) p = trashObj.AddComponent<ES_TrashProjectile>();
            p.Init(dir * throwSpeed, est.Pool, autoReturnAfter);
        }

        return StateStatus.Success;
    }
}
