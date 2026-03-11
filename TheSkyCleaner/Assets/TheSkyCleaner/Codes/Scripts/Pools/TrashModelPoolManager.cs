using System.Collections.Generic;
using UnityEngine;

public class TrashModelPoolManager : ObjectPoolManager
{
    [SerializeField] private List<GameObject> m_prefabs;

    protected override int AddToPool()
    {
        m_prefab = m_prefabs[Random.Range(0, m_prefabs.Count)];
        return base.AddToPool();
    }
}
