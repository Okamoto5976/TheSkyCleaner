using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private MovementHandler m_MovementHandler;
    [SerializeField] private ReturnObjectToPool m_ReturnObjectToPool;

    private Vector3 m_direction;
    private float m_velocity;
    private ILockOnTarget m_target;

    public void InjectDirection(Vector3 direction) => m_direction = direction;
    public void InjectVelocity(float velocity) => m_velocity = velocity;

    public void Initialize()
    {
        m_MovementHandler.SetSpeed(m_velocity);
        transform.rotation = Quaternion.LookRotation(m_direction);
    }

    private void Update()
    {
        m_MovementHandler.MoveAllGlobal(m_direction);

        if (m_target == null) return;

        //float dis = Vector3.Distance
        //    (gameObject.transform.position, m_target.Transform.position);

        //if (dis > 0.5f) return;

        //if(m_target is IDamage idamage)
        //{
        //    idamage.Damage(2);
        //}

    //    Collider[] hits = Physics.OverlapSphere(
    //　　　　transform.position,
   　//　　　 0.5f,
   　//　　　 m_damageLayer
　　　　//);

    //    foreach (var hit in hits)
    //    {
    //        if (hit.TryGetComponent<IDamage>(out var idamage))
    //        {
    //            idamage.Damage(2);
    //            m_ReturnObjectToPool.ReturnToPool();
    //        }
    //    }
    }

    public void SetTarget(ILockOnTarget target) { m_target = target; }
}
