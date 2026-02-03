using UnityEngine;

[CreateAssetMenu(fileName = "ES_SeekTarget", menuName = "Enemy/States/Seek Target")]
public class ES_SeekTarget : EnemyState
{
    [SerializeField] public AxisVector3Container m_playerPosition;

    public AxisVector3Container PlayerPosition => m_playerPosition;

    public override void OnUpdate(float deltaTime)
    {
        Vector3 dir = DirToTarget(m_playerPosition.Value, _transform.position);
        est.SetMoveDirection(dir);
    }
}