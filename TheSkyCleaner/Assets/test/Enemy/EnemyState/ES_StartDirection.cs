using UnityEngine;

[CreateAssetMenu(fileName = "ES_StartDirection", menuName = "Enemy/States/Start Direction (No-Op)")]
public class ES_StartDirection : EnemyState
{
    private Vector3 m_direction;
    [SerializeField] private float m_moveSpeed = 1f;

    public override void OnEnter()
    {
        Vector3 randomDir = Random.onUnitSphere;//ãÖëÃÇÃï\ñ è„Ç…ì_Çï‘Ç∑
        randomDir.Normalize();

        m_direction = randomDir;
    }

    public override void OnUpdate(float deltaTime)
    {
        est.SetMoveSpeed(m_moveSpeed);
        est.SetMoveDirection(m_direction);
    }
}
