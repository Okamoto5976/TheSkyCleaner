using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyThrowContext : MonoBehaviour
{
    public GameObject CurrentTrash;

    public Vector3 TargetSnapshot;

    /// <summary>「そのフレームで一度だけ実行」を保証するためのフラグ群</summary>
    private readonly HashSet<string> _marks = new();

    /// <summary>
    /// 指定キーで「未実行」なら true を返し、同時に実行済みにマークする。
    /// 2回目以降は false（＝処理しない）。
    /// </summary>
    public bool TryMarkOnce(string key) => _marks.Add(key);

    /// <summary>特定ステップの実行済みフラグ解除（必要に応じて）</summary>
    public void ClearMark(string key) => _marks.Remove(key);

    /// <summary>全リセット（必要に応じて）</summary>
    public void ResetAll()
    {
        _marks.Clear();
        CurrentTrash = null;
        TargetSnapshot = default;
    }
}