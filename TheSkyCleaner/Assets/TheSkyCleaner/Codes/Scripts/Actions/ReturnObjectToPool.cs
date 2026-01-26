using UnityEngine;

public class ReturnObjectToPool : MonoBehaviour
{
    private ObjectPoolManager m_poolManager;
    private int m_poolIndex;

    public void InjectPoolManager(ObjectPoolManager poolManager, int poolIndex)
    {
        m_poolManager = poolManager;
        m_poolIndex = poolIndex;
    }

    public void ReturnToPool()
    {
        m_poolManager.ReturnToPool(m_poolIndex);
    }
}
