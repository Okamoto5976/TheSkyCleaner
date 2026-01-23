using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ComponentPoolHandler<T> : ObjectPoolManager
{
    protected List<T> m_objectComponent;

    protected override void Awake()
    {
        m_objectComponent = new();
        base.Awake();
    }

    public T GetComponentFromPool()
    {
        return GetFromPool(m_objectComponent);
    }

    protected override int AddToPool()
    {
        int index = base.AddToPool();
        m_objectComponent.Add(m_objectPool.ElementAt(index).GetComponent<T>());
        return index;
    }

    public IReadOnlyList<T> GetActiveComponent()
    {
        IEnumerable<T> tmp = m_inUseQue.Select(i => m_objectComponent.ElementAt(i));
        return tmp.ToList();
    }
}
