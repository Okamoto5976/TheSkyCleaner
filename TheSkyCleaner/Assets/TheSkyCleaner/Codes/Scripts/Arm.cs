using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Arm : MonoBehaviour
{
    public enum State
    {
        Idle,
        Moving,
        Returning
    }

    [SerializeField] private Transform m_player;


    private State m_state  = State.Idle;

    private Transform m_targetEnemy;
    private Transform m_returnPoint;

    [SerializeField] private float m_speed = 6.0f;

    private void Start()
    {
        m_returnPoint = m_player;
    }

    private void Update()
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

    public void MoveToEnemy(Transform enemy)
    {
        m_targetEnemy= enemy;
        transform.SetParent(null);
        m_state = State.Moving;
    }


    public void Move()
    {
        if(m_targetEnemy == null)
        {
            Return();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            m_targetEnemy.position,
            m_speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, m_targetEnemy.position) < 0.05f)
        {
            m_state = State.Returning;
        }
    }

    public void Return()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            m_returnPoint.position,
            m_speed * Time.deltaTime);
        Debug.Log("return");

        if(Vector3.Distance(transform.position, m_returnPoint.position) < 0.05f)
        {
            transform.SetParent(m_returnPoint.parent);
            m_state = State.Idle;
        }
    }
}
