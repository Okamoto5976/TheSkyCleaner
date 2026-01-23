using System.Collections.Generic;
using System.Linq;

public class EnemyPoolManager : ObjectPoolManager
{
    protected List<T_Enemy> m_objectEnemy;

    protected override void Awake()
    {
        m_objectEnemy = new();
        base.Awake();
    }

    public T_Enemy GetEnemyFromPool()
    {
        return GetFromPool(m_objectEnemy);
    }

    protected override int AddToPool()
    {
        int index = base.AddToPool();
        m_objectEnemy.Add(m_objectPool.ElementAt(index).GetComponent<T_Enemy>());
        return index;
    }

    public IReadOnlyList<T_Enemy> GetActiveEnemies()
    {
        IEnumerable<T_Enemy> tmp = m_inUseQue.Select(i => m_objectEnemy.ElementAt(i));
        return tmp.ToList();
    }
}
