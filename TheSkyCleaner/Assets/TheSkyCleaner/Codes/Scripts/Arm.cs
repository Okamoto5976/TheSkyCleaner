using UnityEngine;

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

    private State m_state  = State.Idle;

    private Transform m_targetEnemy;
    private Transform m_transform;
    private Vector3 m_returnPosition;

    private float m_speed;

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
                Move();
                break;
            case State.Returning:
                Return();
                break;
        }

        Debug.Log(m_state);
    }

    public void MoveToEnemy(Transform enemy,float speed)
    {
        m_speed = speed;
        m_targetEnemy= enemy;
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
            m_targetEnemy.position,
            m_speed);

        if(Vector3.Distance(m_transform.position, m_targetEnemy.position) < 0.05f)
        {
            m_state = State.Returning;
        }
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
            m_controller.Return(this);

            m_transform.localPosition = m_returnPosition;
        }
    }
}
