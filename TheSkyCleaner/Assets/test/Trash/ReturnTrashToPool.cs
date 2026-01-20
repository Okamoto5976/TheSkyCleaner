using UnityEngine;

public class ReturnTrashtToPool : MonoBehaviour
{
    private TrashPoolManager m_trashManager;
    private TEManager m_teManager;

    private void Awake()
    {
        m_teManager = GetComponent<TEManager>();
    }

    public void InjectPoolManager(TrashPoolManager trashManager)
    {
        m_trashManager = trashManager;
    }

    public void ReturnToPool()
    {
        m_trashManager.ReturnToPool(m_teManager);
    }
}
