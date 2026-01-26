using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arm : MonoBehaviour
{
    public enum State
    {
        Idle,
        Moving,
        Returning
    }

    [SerializeField] private Transform m_player;
    [SerializeField] private ArmController m_controller;
    [SerializeField] private Camera m_camera;

    private State m_state  = State.Idle;
    
    private GameObject m_targetEnemy;
    private Transform m_targetTransform;
    private Transform m_transform;
    private Vector3 m_returnPosition;

    private int m_index;
    private float m_speed;
    private int m_id;

    private void Start()
    {
        m_transform = transform;
        m_returnPosition = m_transform.localPosition;
    }

    private void FixedUpdate()
    {
        switch(m_state)
        {
            case State.Moving:

                if(!IsTarget())
                {
                    m_state = State.Returning;
                    return;
                }

                Move();
                break;
            case State.Returning:
                Return();
                break;
        }

        //Debug.Log(m_state);
    }

    private bool IsTarget()
    {
        if(m_targetEnemy == null) return false;
        if(!m_targetEnemy.gameObject.activeSelf) return false;

        //   ---  z軸０より後ろでfalse  ---
        Vector3 sp = m_camera.WorldToScreenPoint(m_targetTransform.position);
        if (sp.z < 0) return false;

        if (Vector3.Distance(m_transform.position, m_targetTransform.position) < 0.5f)
        {
            //もしゴミなら素材回収。　敵ならダメージを　インターフェースで

            //素材回収のさいSOの中身にMaterial1..2..3　となるので変数を1.2　と用意することで
            //配列で入れられるね！
            return false;
        }

        return true;
    }

    public void MoveToEnemy(GameObject enemy,Transform enemypositionm, float speed,int ID,int index)
    {
        //返すための値
        m_index = index;
        m_id = ID;

        m_speed = speed;
        m_targetEnemy = enemy;
        m_targetTransform = enemypositionm;
        m_transform.SetParent(null);
        m_state = State.Moving;
    }


    public void Move()
    {
        if(m_targetEnemy == null)
        {
            Return();
            return;
        }

        m_transform.position = Vector3.MoveTowards(
            m_transform.position,
            m_targetTransform.position,
            m_speed);
    }

    public void Return()
    {
        m_transform.position = Vector3.MoveTowards(
            m_transform.position,
            m_player.position + m_returnPosition,
            m_speed);
        Debug.Log("return");

        if(Vector3.Distance(m_transform.position, m_player.position + m_returnPosition) < 0.05f)
        {
            m_transform.SetParent(m_player.parent);
            m_state = State.Idle;
            m_controller.Return(m_id,m_index);

            m_transform.localPosition = m_returnPosition;
        }
    }
}
