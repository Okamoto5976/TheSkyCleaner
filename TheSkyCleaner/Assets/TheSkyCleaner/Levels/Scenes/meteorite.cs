using UnityEngine;

public class meteorite : MonoBehaviour
{
    private float m_speed;
    private Vector3 m_SlideDiretion;


    private SphereCollider m_collider;

    [SerializeField] private HealthContainer m_playerHealth;
    [SerializeField] private AxisVector3Container m_playerPos;

    [SerializeField] private int m_attack;

    private void Awake()
    {
        m_collider = gameObject.GetComponent<SphereCollider>();
    }

    public void Setup(float speed, Vector3 direction)
    {
        m_speed=speed;
       m_SlideDiretion =direction;
    }
    void Update()
    {
     transform.Translate(m_SlideDiretion*m_speed*Time.deltaTime);   

        if(transform.position.magnitude>150f)
        {
            Destroy(gameObject);
        }

        //“–‚½‚è”»’è
        float dis = Vector3.Distance(gameObject.transform.position, m_playerPos.Value);
        Debug.Log(dis);
        if (dis < m_collider.radius)
        {
            m_playerHealth.Damage(m_attack);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.TryGetComponent<fuel1>(out fuel1 player))
    //    {
    //        player.UseFuel(9999f);
    //        //Debug.Log("ˆêŒ‚•KEŠZ‘³ˆêG—ô‹óŠW¢‹SšLšaša");
    //    }
    //}
}
