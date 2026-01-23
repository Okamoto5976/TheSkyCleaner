using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] protected Logger m_logger;
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private int m_poolCount;
    [SerializeField] private bool m_forcePoolCount = true;

    protected List<ReturnObjectToPool> m_objectPool;
    protected List<int> m_inUseQue;

    private Transform m_transform;

    protected virtual void Awake()
    {
        m_objectPool = new();
        m_inUseQue = new();
        m_transform = transform;
        for (int i = 0; i < m_poolCount; i++)
        {
            AddToPool();
        }
    }

    public GameObject GetObjectFromPool()
    {
        return GetFromPool(m_objectPool).gameObject;
    }

    protected T GetFromPool<T>(List<T> list)
    {
        int index = GetIndexFromPool();
        T obj = list.ElementAt(index);
        m_inUseQue.Add(index);
        return obj;
    }

    protected int GetIndexFromPool()
    {
        var x = m_objectPool
            .Select((obj, index) => new { obj, index })
            .FirstOrDefault(x => !m_inUseQue.Contains(x.index));
        int index;
        if (x == null)
        {
            if (m_forcePoolCount)
            {
                index = m_inUseQue.First();
                m_inUseQue.RemoveAt(0);
            }
            else
            {
                index = AddToPool();
            }
        }
        else
        {
            index = x.index;
        }
        return index;
    }

    public void ReturnToPool(int index)
    {
        ReturnObjectToPool obj = m_objectPool.ElementAt(index);
        m_inUseQue.Remove(index);
        obj.transform.parent = m_transform;
        obj.gameObject.SetActive(false);
    }

    protected virtual int AddToPool()
    {
        GameObject obj = Instantiate(m_prefab, m_transform);
        obj.SetActive(false);
        
        int index = m_objectPool.Count();
        obj.name += index;

        ReturnObjectToPool rotp = obj.GetComponent<ReturnObjectToPool>();
        rotp.InjectPoolManager(this, index);
        m_objectPool.Add(rotp);
        return index;
    }

}
