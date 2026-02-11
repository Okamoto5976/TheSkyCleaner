using UnityEngine;

/// すべての State が継承する抽象クラス（ScriptableObject）
/// ここに共通ユーティリティ（移動、ターゲット参照、停止など）を定義。
public abstract class EnemyState : ScriptableObject
{
    protected EnemyStateMachine est;
    protected GameObject _gameObject;
    protected Transform _transform;

    public void InjectVariables(EnemyStateMachine stateMachine)
    {
        _gameObject = stateMachine.gameObject;
        _transform = _gameObject.transform;
        est = stateMachine;
    }

    ///ステート開始時に一度だけ呼ばれる</summary>
    public virtual void OnEnter() { }

    ///毎フレーム実行。継続なら Running、完了で Success
    public abstract void OnUpdate(float deltaTime);

    /// <summary>ステート終了時に一度だけ呼ばれる</summary>
    public virtual void OnExit() { }

    // ====== 共通ユーティリティ ======

    //protected void Move(EnemyStateMachine est, Vector3 worldDirection)
    //    => est.MoveDirection(worldDirection);

    protected Transform GetTarget()
        => est.Target;

    protected Vector3 DirToTarget(Vector3 targetPosition, Vector3 myPosition)
    {
        if (targetPosition == null) return Vector3.zero;

        Vector3 dir = targetPosition - myPosition;
        return dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.zero;
    }
}