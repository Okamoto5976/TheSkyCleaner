using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class LargeTrashController : MonoBehaviour, ILockOnTarget,IDamage
{
    [SerializeField] private AxisVector3Container m_target;
    [SerializeField] private CollectSO m_collectSO;
    [SerializeField] private DropSO m_dropSO;

    //[SerializeField] private ObjectPoolManager m_trashpool; // Inspector ‚ÅŠ„‚è“–‚Ä

    [Header("Refalence")]
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private ReturnObjectToPool m_returnObjectToPool;
    private ObjectPoolManager m_poolTrash;

    [Header("Movement")]
    [System.NonSerialized] public float m_moveSpeed;
    [System.NonSerialized] public Vector3 m_direction;

    private Vector3 m_initDir;

    private bool m_isMove = false;

    private Transform m_transform;
    private int m_attack;
    private int m_hp;

    public ObjectPoolManager PoolTrash => m_poolTrash;

    public Transform Transform => m_transform;

    public GameObject GameObject => gameObject;

    public DropSO GetDropData() => m_dropSO;

    private void Awake()
    {
        m_transform = transform;
        m_attack = m_collectSO.Attack;
        m_hp = m_collectSO.HP;
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

        if (dis < trash.radius || gameObject.transform.position.z <= m_target.Value.z - 5)
        {
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }
    }

    public void SetPoolObj(ObjectPoolManager t) => m_poolTrash = t;


    public void Damage(int damage)
    {
        Debug.Log("largetrash damage");
        m_hp -= damage;
        
        if(m_hp <= 0)
        {
            m_returnObjectToPool.ReturnToPool();
            //¬‚³‚¢ƒSƒ~¶¬
            m_poolTrash.GetObjectFromPool();//ˆÊ’uŽw’è‚µ‚Ä‚È‚¢
        }
    }
}
