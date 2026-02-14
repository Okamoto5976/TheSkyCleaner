using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPoolManager : ComponentPoolHandler<T_Enemy>
{
    public IReadOnlyList<Vector3> GetEnemyPositions()
    {
        return m_inUseQue
            .Select(x => m_objectPool[x].transform.position).ToList();
    }
}
