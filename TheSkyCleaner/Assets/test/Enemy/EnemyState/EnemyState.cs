using UnityEngine;

public enum StateStatus
{
    Running,
    Success,
    Failure
}

/// すべての State が継承する抽象クラス（ScriptableObject）
/// ここに共通ユーティリティ（移動、ターゲット参照、停止など）を定義。
public abstract class EnemyState : ScriptableObject
{
    ///ステート開始時に一度だけ呼ばれる</summary>
    public virtual void OnEnter(EnemyStateMachine est) { }

    ///毎フレーム実行。継続なら Running、完了で Success
    public abstract StateStatus OnUpdate(EnemyStateMachine est, float deltaTime);

    /// <summary>ステート終了時に一度だけ呼ばれる</summary>
    public virtual void OnExit(EnemyStateMachine est) { }

    // ====== 共通ユーティリティ ======

    //protected void Move(EnemyStateMachine est, Vector3 worldDirection)
    //    => est.MoveDirection(worldDirection);

    protected void Stop(EnemyStateMachine est)
        => est.MoveDirection(Vector3.zero);

    protected Transform GetTarget(EnemyStateMachine est)
        => est.Target;

    //protected Vector3 DirToTarget(EnemyStateMachine est, bool ignoreY = false)
    //{
    //    var t = GetTarget(est);
    //    if (t == null) return Vector3.zero;

    //    Vector3 dir = t.position - est.transform.position;
    //    if (ignoreY) dir.y = 0f;
    //    return dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.zero;
    //}
}