using UnityEngine;

[CreateAssetMenu(fileName = "ES_GiveDirection", menuName = "Enemy/States/Give Direction (No-Op)")]
public class ES_GiveDirection : EnemyState
{
    [SerializeField] private Vector3 m_direction = Vector3.forward;
    [SerializeField] private float m_moveSpeed = 1f;
 
    public override void OnUpdate(float deltaTime)
    {
        Debug.Log(m_direction);
        est.SetMoveSpeed(m_moveSpeed);
        est.SetMoveDirection(m_direction);
    }
}