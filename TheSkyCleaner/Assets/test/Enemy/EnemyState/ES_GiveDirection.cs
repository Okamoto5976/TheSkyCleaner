using UnityEngine;

/// 進行方向を与える
/// ここでは State 自体は即時 Success とし、方向は Machine 側に保持しないため、
/// 通常はこのあとに Move 系のステートを連ねる設計を想定している。
/// 
/// （もし方向の共有が必要なら、Machine に currentDirection を保持するフィールドを足して、
/// ここで設定 → MoveStraight でそれを読む…などの方針に変更してください。）
/// 
/// 本サンプルでは「GiveDirection → MoveStraight」の連結を想定します。
/// MoveStraight へは本クラスの direction をコピー渡しできるよう、
/// MoveStraight 側にも同じ値を直指定するか、
/// あるいは ES_GiveDirection を省略して、MoveStraight の inspector で方向を直接設定。
[CreateAssetMenu(fileName = "ES_GiveDirection", menuName = "Enemy/States/Give Direction (No-Op)")]
public class ES_GiveDirection : EnemyState
{
    [SerializeField] private Vector3 m_direction = Vector3.forward;

    private MovementHandler m_movementHandler;
    public Vector3 m_Direction => m_direction.normalized;

    public override void OnEnter(EnemyStateMachine est)
    {
        m_movementHandler = new MovementHandler();
    }
    public override StateStatus OnUpdate(EnemyStateMachine est, float deltaTime)
    {
        m_movementHandler.m_speeds = m_direction;
        return StateStatus.Success;
    }
}