using UnityEngine;

[RequireComponent(typeof(MovementHandler),typeof(ReturnObjectToPool))]
public class HumanController : MonoBehaviour, ILockOnTarget
{
    [SerializeField] private CollectSO m_collectSO;
    [SerializeField] private Vector3 m_moveDirection = Vector3.down;
    [SerializeField] private Vector3 m_reticleOffset = Vector3.zero;
    public Vector3 ReticleOffset => m_reticleOffset;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    private MovementHandler m_movementHandler;
    private ReturnObjectToPool m_returnObjectToPool;

    private void Awake()
    {
        m_movementHandler = GetComponent<MovementHandler>();
        m_returnObjectToPool = GetComponent<ReturnObjectToPool>();
    }

    private void Update()
    {
        m_movementHandler.MoveAllGlobal(m_moveDirection);
    }

    public void ReturnToPool()
    {
        m_returnObjectToPool.ReturnToPool();
    }

    public DropSO Collect()
    {
        DropSO drop = GetDropData();
        ReturnToPool();
        return drop;
    }

    public DropSO GetDropData() => m_collectSO.Drop;
}
