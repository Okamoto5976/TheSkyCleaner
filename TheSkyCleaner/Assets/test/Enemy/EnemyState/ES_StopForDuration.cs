using UnityEngine;

[CreateAssetMenu(fileName = "ES_StopForDuration", menuName = "Enemy/States/Stop For Duration")]
public class ES_StopForDuration : EnemyState
{
    EnemySequence.StateMachineState es = new EnemySequence.StateMachineState();
    private float _timer;


    public override void OnEnter(EnemyStateMachine est)
    {
        _timer = 0f;
        // 停止開始：必要ならエフェクトやアニメ開始など
        Stop(est);
    }

    public override StateStatus OnUpdate(EnemyStateMachine est, float deltaTime)
    {
        _timer += deltaTime;

        // 停止中：毎フレームも停止を維持
        Stop(est);

        if (_timer >= es.m_time)
        {
            return StateStatus.Success;
        }
        return StateStatus.Running;
    }

}
