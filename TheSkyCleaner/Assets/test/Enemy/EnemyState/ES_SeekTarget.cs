using UnityEngine;

[CreateAssetMenu(fileName = "ES_SeekTarget", menuName = "Enemy/States/Seek Target")]
public class ES_SeekTarget : ES_GiveDirection
{
    [SerializeField] public AxisVector3Container m_playerPosition;

    public AxisVector3Container PlayerPosition => m_playerPosition;

    public override void OnUpdate(float deltaTime)
    {
        m_direction = DirToTarget(m_playerPosition.Value, _transform.position);
        base.OnUpdate(deltaTime);
    }
}