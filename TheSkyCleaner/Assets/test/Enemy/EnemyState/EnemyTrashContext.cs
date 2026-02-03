using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyThrowContext : MonoBehaviour
{
    public GameObject CurrentTrash;

    public Vector3 TargetSnapshot;

    private readonly HashSet<string> _marks = new();

    public bool TryMarkOnce(string key) => _marks.Add(key);

    public void ClearMark(string key) => _marks.Remove(key);

    public void ResetAll()
    {
        _marks.Clear();
        CurrentTrash = null;
        TargetSnapshot = default;
    }
}