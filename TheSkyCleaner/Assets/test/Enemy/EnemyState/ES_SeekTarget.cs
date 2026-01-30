using UnityEngine;

[CreateAssetMenu(fileName = "ES_SetMoveDirFromTarget", menuName = "Enemy/States/Set Move Direction From Target")]
public class ES_SeekTarget : EnemyState
{
    [Tooltip("Yを無視して水平方向だけを渡すなら true")]
    [SerializeField] private bool ignoreY = false;

    EnemySequence.StateMachineState es = new EnemySequence.StateMachineState();
    private float _timer;

    public override void OnEnter(EnemyStateMachine est)
    {
        _timer = 0f;
    }

    public override StateStatus OnUpdate(EnemyStateMachine est, float deltaTime)
    {
        _timer += deltaTime;

        var t = GetTarget(est);
        if (t == null) return StateStatus.Failure;

        Vector3 dir = t.position - est.transform.position;
        if (ignoreY) dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f) dir.Normalize(); else dir = Vector3.zero;

        // 方向ベクトルだけを MovementHandler に渡す
        // ※ EnemyStateMachine.MoveDirection は「方向 or 速度」両対応ラッパですが、
        //    本ケースでは「方向」を渡す前提で、est 側のフラグ(m_moveAllTakesDirection)を true にしてください。
        est.MoveDirection(dir);

        return (_timer >= es.Time) ? StateStatus.Success : StateStatus.Running;
    }
}