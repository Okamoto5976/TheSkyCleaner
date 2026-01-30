using UnityEngine;

[CreateAssetMenu(fileName = "ES_SetMoveDirFromTarget", menuName = "Enemy/States/Set Move Direction From Target")]
public class ES_SeekTarget : EnemyState
{
    [SerializeField] private AxisVector3Container m_playerPosition;

    private float _timer;

    public override void OnEnter()
    {
        _timer = 0f;
    }

    public override void OnUpdate(float deltaTime)
    {
        Vector3 dir = DirToTarget(m_playerPosition.Value, _transform.position);
        est.SetMoveDirection(dir);
    }
}