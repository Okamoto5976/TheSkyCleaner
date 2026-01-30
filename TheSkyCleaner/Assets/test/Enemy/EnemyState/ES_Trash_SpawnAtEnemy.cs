using UnityEngine;

/// <summary>
/// Pull して保持している Trash を、敵の現在位置にスポーン（アクティブ化）する State。
/// 既にアクティブなら何もしない。
/// </summary>
[CreateAssetMenu(fileName = "ES_Trash_SpawnAtEnemy", menuName = "Enemy/States/Trash/Spawn At Enemy")]
public class ES_Trash_SpawnAtEnemy : EnemyState
{
    private const string MarkKey = "Spawned";

    public override void OnUpdate(float deltaTime)
    {
        var ctx = GetOrAddContext();
        if (ctx.CurrentTrash == null) return;

        // 同じウィンドウで一度だけ実行
        if (!ctx.TryMarkOnce(MarkKey)) return;

        var t = ctx.CurrentTrash.transform;
        t.position = _transform.position;
        // 必要なら初期回転も設定
        // t.rotation = Quaternion.identity;

        if (!ctx.CurrentTrash.activeSelf)
            ctx.CurrentTrash.SetActive(true);
    }

    private EnemyThrowContext GetOrAddContext()
    {
        var ctx = _gameObject.GetComponent<EnemyThrowContext>();
        if (ctx == null) ctx = _gameObject.AddComponent<EnemyThrowContext>();
        return ctx;
    }
}
