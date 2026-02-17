using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class LargeTrashController : MonoBehaviour, ILockOnTarget,IDamage
{
    [SerializeField] private CollectSO m_collectSO;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private FloatContainer m_fuel;

    [Header("Refalence")]
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private ReturnObjectToPool m_returnObjectToPool;
    private TrashPoolManager m_poolTrash;

    [Header("Movement")]
    [System.NonSerialized] public float m_moveSpeed;
    [System.NonSerialized] public Vector3 m_direction;

    private Vector3 m_initDir;

    private bool m_isMove = false;

    private Transform m_transform;
    private SphereCollider m_collider;
    private int m_attack;
    private int m_hp;

    [Header("Trash")]
    //[SerializeField] private int m_trashSpawn;
    [SerializeField] private int m_trashSpeed;

    private TrashController m_trash; //小さいゴミ本体
    public Transform Transform => m_transform;
    public GameObject GameObject => gameObject;
    public DropSO GetDropData() => m_dropSO;

    private void Awake()
    {
        m_collider = GetComponent<SphereCollider>();
        m_transform = transform;
    }

    private void OnEnable()
    {
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

        //当たり判定
        float dis = Vector3.Distance(gameObject.transform.position, m_playerPos.Value);

        if (dis < m_collider.radius) 
        {
            var fuel = Mathf.Max(0f, m_fuel.Value - m_attack);//ダメージ
            m_fuel.SetValue(fuel);
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }

        if(gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            m_returnObjectToPool.ReturnToPool();
            m_movementHandler.MoveAll(m_initDir);
        }
    }

    public void SetPoolObj(TrashPoolManager t) => m_poolTrash = t;


    public void Damage(int damage)
    {
        m_hp -= damage;
        
        if(m_hp <= 0)
        {
            m_trash = m_poolTrash.GetComponentFromPool();//位置指定してない
                                                            //生成位置 
            m_trash.transform.position = this.transform.position;
            m_trash.gameObject.SetActive(true);
            m_trash.SetMoving(true);
            m_trash.SetMoveSpeed(m_trashSpeed);
            Vector3 dir = new Vector3(0, 0, -1);
            m_trash.SetMoveDirection(dir);

            //for (int i = 0; i < m_trashSpawn; i++)
            //{
                

            //}

            m_returnObjectToPool.ReturnToPool(); 
        }
    }
}
