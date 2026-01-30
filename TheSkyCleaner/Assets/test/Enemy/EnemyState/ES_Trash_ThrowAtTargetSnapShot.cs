using UnityEngine;

/// <summary>
/// 投げる瞬間のターゲット位置をスナップショットし、その方向へ Trash を投げる State。
/// 1回だけ速度を与えて投擲する。
/// </summary>
[CreateAssetMenu(fileName = "ES_Trash_ThrowAtTargetSnapshot", menuName = "Enemy/States/Trash/Throw At Target Snapshot")]
public class ES_Trash_ThrowAtTargetSnapshot : EnemyState
{
    [Header("Throw Settings")]
    [SerializeField, Tooltip("初速（m/s）")] private float m_initialSpeed = 12f;
    [SerializeField, Tooltip("ターゲットが居ない場合の代替方向（ローカル基準ではなくワールド基準）")]
    private Vector3 m_fallbackDirection = Vector3.forward;

    private const string MarkKey = "Thrown";

    public override void OnUpdate(float deltaTime)
    {
        var ctx = GetOrAddContext();
        if (ctx.CurrentTrash == null) return;

        // 同じウィンドウで一度だけ実行
        if (!ctx.TryMarkOnce(MarkKey)) return;

        var trashGO = ctx.CurrentTrash;
        var rb = trashGO.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("[ES_Trash_ThrowAtTargetSnapshot] Trash に Rigidbody が必要です。");
            // 後続の混乱を避けるため参照はクリア
            ctx.CurrentTrash = null;
            return;
        }

        // 「投げた瞬間」のターゲット位置をスナップショット
        var target = GetTarget();
        Vector3 targetPos;
        if (target != null)
        {
            targetPos = target.position;
        }
        else
        {
            // ターゲット不在時は前方に投げる
            targetPos = trashGO.transform.position + (m_fallbackDirection.sqrMagnitude > 0.0001f
                ? m_fallbackDirection.normalized
                : Vector3.forward) * 5f;
        }
        ctx.TargetSnapshot = targetPos;

        // 向きと速度ベクトルを決定（シンプルな直進投擲）
        Vector3 from = trashGO.transform.position;
        Vector3 dir = (ctx.TargetSnapshot - from);
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = (m_fallbackDirection.sqrMagnitude > 0.0001f ? m_fallbackDirection : Vector3.forward);
        }
        dir = dir.normalized;

        // もし必要なら、ここで放物投射（重力あり）用の初速ベクトル計算に置き換える
        // 例: 目標までの水平距離と重力から打ち上げ角を決める など

        // 速度を付与
        rb.isKinematic = false;
        rb.velocity = dir * m_initialSpeed;

        // このステップ完了後は参照をクリアして多重実行を防止
        ctx.CurrentTrash = null;
    }

    private EnemyThrowContext GetOrAddContext()
    {
        var ctx = _gameObject.GetComponent<EnemyThrowContext>();
        if (ctx == null) ctx = _gameObject.AddComponent<EnemyThrowContext>();
        return ctx;
    }
}