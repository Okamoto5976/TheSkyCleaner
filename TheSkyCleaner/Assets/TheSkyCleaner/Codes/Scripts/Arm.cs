using UnityEngine;
using UnityEngine.InputSystem;

public class Arm : MonoBehaviour
{
    private enum ArmType
    {
        Right,
        Left
    }

    [SerializeField] private Transform PlayerPosition;
    [SerializeField] private float m_speed = 6.0f;
    private Vector3 m_dir;
    private Vector3 m_playerposition;

    private Ray m_ray;
    private bool m_ismove = false;
    private int timer;

    private void Update()
    {
        m_playerposition = PlayerPosition.position;

        m_ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Vector3 target;
        if(Physics.Raycast(m_ray, out RaycastHit hit))
        {
            target = hit.point;
        }
        else
        {
            target = m_ray.GetPoint(100f);
        }
        m_dir = (target - transform.position).normalized;

        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("ƒA[ƒ€");
            m_ismove=true;
        }

        if(m_ismove == true) ArmMove();
        if(m_ismove == false) ArmReturn();
    }

    private void Awake()
    {
        
    }

    public void ArmMove()
    {
        if (timer > 1000)
        {
            this.gameObject.transform.parent = null;
            m_ismove = false;
        }
        timer++;
        transform.position += m_dir * m_speed * Time.deltaTime;
    }

    public void ArmReturn()
    {
        Debug.Log("return");
        timer = 0;
        transform.position = Vector3.MoveTowards
            (transform.position,
            m_playerposition,
            m_speed * Time.deltaTime);
    }
}
