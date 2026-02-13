using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class TrashController : MonoBehaviour, ILockOnTarget
{
    [SerializeField] private AxisVector3Container m_target;

    [Header("Refalence")]
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private ReturnObjectToPool m_returnObjectToPool;

    [Header("Movement")]
    [System.NonSerialized] public float m_moveSpeed;
    [System.NonSerialized] public Vector3 m_direction;

    private Vector3 m_initDir;

    private bool m_isMove = false;

    private Transform m_transform;
    public Transform Transform => m_transform;

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        m_transform = transform;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        m_moveSpeed = moveSpeed;
        m_movementHandler.SetSpeed((float)m_moveSpeed);
    }

    public void SetMoveDirection(Vector3 direction) => m_direction = direction;

    public void SetMoving(bool isMobing) => m_isMove = isMobing;

    public void Initialized(Vector3 dir) => m_initDir = dir;

    private void FixedUpdate()
    {
        m_movementHandler.MoveAll(m_direction);

        //‹¤’Ê•”•ª
        var trash = gameObject.GetComponent<SphereCollider>();

        //“–‚½‚è”»’è
        float dis = Vector3.Distance(gameObject.transform.position, m_target.Value);

        if(dis < trash.radius || gameObject.transform.position.z <= m_target.Value.z - 5)
        {
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }
    }

    public DropSO GetDropData()
    {
        throw new System.NotImplementedException();
    }
}


