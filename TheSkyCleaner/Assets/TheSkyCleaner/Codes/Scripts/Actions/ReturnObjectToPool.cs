using UnityEngine;

public class ReturnObjectToPool : MonoBehaviour
{
    private ObjectPoolManager m_poolManager;

    public void InjectPoolManager(ObjectPoolManager poolManager)
    {
        m_poolManager = poolManager;
    }

    public void ReturnToPool()
    {
        m_poolManager.ReturnToPool(gameObject);
    }
}
