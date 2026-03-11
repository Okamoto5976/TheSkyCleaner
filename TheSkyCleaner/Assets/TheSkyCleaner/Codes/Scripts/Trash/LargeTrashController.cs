using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class LargeTrashController : MonoBehaviour, ILockOnTarget,IDamage
{
    private CollectSO m_collectSO;

    [SerializeField] private AxisVector3Container m_playerPos;
    [SerializeField] private HealthContainer m_playerHealth;

    [Header("Refalence")]
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private ReturnObjectToPool m_returnObjectToPool;
    private TrashManager m_trashManager;
    private TrashPoolManager m_poolTrash;
    private ObjectPoolManager m_poolDeathParticle;

    [Header("Movement")]
    [System.NonSerialized] public float m_moveSpeed;
    [System.NonSerialized] public Vector3 m_direction;

    [SerializeField] private AudioContainer m_deathSound;

    private Vector3 m_initDir;

    private bool m_isMove = false;

    private Transform m_transform;
    private SphereCollider m_collider;
    private int m_attack;
    private int m_hp;

    [Header("Trash")]
    //[SerializeField] private int m_trashSpawn;
    [SerializeField] private int m_trashSpeed;

    private int m_index; 

    private TrashController m_trash; //小さいゴミ本体
    public Transform Transform => m_transform;
    public GameObject GameObject => gameObject;
    public DropSO GetDropData() => m_collectSO.Drop;
    [SerializeField] private Vector3 m_reticleOffset;

    [SerializeField] private Transform m_root;

    private ReturnObjectToPool m_visualreturn;

    private float m_moveTime = 2f;

    public Vector3 ReticleOffset => m_reticleOffset;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_collider = GetComponent<SphereCollider>();
        m_transform = transform;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveTime());
    }

    private IEnumerator MoveTime()
    {
        yield return new WaitForSeconds(m_moveTime);

        SetMoveDirection(m_initDir);
        yield break;
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
            m_playerHealth.Damage(m_attack);
            ReturnToPool();
        }

        if(gameObject.transform.position.z <= m_playerPos.Value.z - 5)
        {
            ReturnToPool();
        }
    }

    public void SetPoolObj(TrashManager t) => m_trashManager = t;
    public void SetPoolDeathEffect(ObjectPoolManager t) => m_poolDeathParticle = t;
    public void SetAudioSource(AudioSource audioSource) => m_audioSource = audioSource;


    public void Damage(int damage)
    {
        m_hp -= damage;
        
        if(m_hp <= 0)
        {
            var obj = m_trashManager.SetThrow(m_index);
            //m_trash = m_poolTrash.GetComponentFromPool();//位置指定してない
            //                                                //生成位置 
            //m_trash.transform.position = this.transform.position;
            //m_trash.gameObject.SetActive(true);
            //m_trash.SetMoving(true);
            //m_trash.SetMoveSpeed(m_trashSpeed);
            //Vector3 dir = new Vector3(0, 0, -1);
            //m_trash.SetMoveDirection(dir);

            obj.transform.position = this.transform.position;
            obj.gameObject.SetActive(true);
            //for (int i = 0; i < m_trashSpawn; i++)
            //{


            //}

            GameObject deathParticle = m_poolDeathParticle.GetObjectFromPool();
            deathParticle.transform.position = Transform.position;
            deathParticle.SetActive(true);

            m_audioSource.PlayOneShot(m_deathSound.AudioClip, m_deathSound.Volume);

            ReturnToPool();
        }
    }

    public void SetVisual(ObjectPoolManager pool, int index)
    {
        //return����
        if (m_visualreturn != null)
        {
            m_visualreturn.ReturnToPool();
            m_visualreturn = null;
        }

        //visual�K��
        GameObject visual = pool.GetObjectFromPool();

        visual.transform.SetParent(m_root);
        visual.transform.localPosition = Vector3.zero;
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale = Vector3.one;

        visual.SetActive(true);
        m_index = index;

        m_visualreturn = visual.GetComponent<ReturnObjectToPool>();
    }

    public void SetStatsData(CollectSO collectSO)
    {
        m_collectSO = collectSO;
        m_attack = m_collectSO.Attack;
        m_hp = m_collectSO.HP;
    }

    private void ReturnToPool()
    {
        m_returnObjectToPool.ReturnToPool();
        m_movementHandler.MoveAll(m_initDir);
    }

    public DropSO Collect()
    {
        DropSO drop = GetDropData();
        ReturnToPool();
        return drop;
    }

    public bool TryCollect(int damage)
    {
        m_hp -= damage;
        return m_hp - damage <= 0;
    }
}
