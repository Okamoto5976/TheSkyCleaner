using UnityEngine;

/// <summary>
/// TrashPool からゴミ（1個）を取得してコンテキストに保持する State。
/// 既に保持中なら何もしない（多重取得を防止）。
/// </summary>
[CreateAssetMenu(fileName = "ES_Trash_PullFromPool", menuName = "Enemy/States/Trash/Pull From Pool")]
public class ES_Trash_PullFromPool : EnemyState
{

    private ObjectPoolManager _trashPool; // 実行時にタグで見つけてキャッシュ

    public override void OnUpdate(float deltaTime)
    {
        var ctx = GetOrAddContext();
        if (ctx.CurrentTrash != null) return; // 既に取得済み

        //// プール取得
        //if (_trashPool == null)
        //{
        //}

        // 1つ取得（ObjectPoolManager 側で非アクティブのまま返ってくる想定）
        var trash = _trashPool.GetObjectFromPool();

        ctx.CurrentTrash = trash;
    }

    private EnemyThrowContext GetOrAddContext()
    {
        var ctx = _gameObject.GetComponent<EnemyThrowContext>();
        if (ctx == null) ctx = _gameObject.AddComponent<EnemyThrowContext>();
        return ctx;
    }
}