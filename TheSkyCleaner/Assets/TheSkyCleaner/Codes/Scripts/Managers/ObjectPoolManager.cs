using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private int m_poolCount;
    [SerializeField] private bool m_forcePoolCount = true;

    private List<GameObject> m_objectPool;
    private List<GameObject> m_inUseQue;

    private Transform m_transform;

    private void Awake()
    {
        m_objectPool = new();
        m_inUseQue = new();
        m_transform = transform;
        for (int i = 0; i < m_poolCount; i++)
        {
            AddToPool();
        }
    }

    public GameObject GetFromPool(bool setActive = false)
    {
        GameObject obj = m_objectPool.FirstOrDefault(x => !m_inUseQue.Contains(x));
        if (obj == null)
        {
            if (m_forcePoolCount)
            {
                obj = m_inUseQue.First();
                m_inUseQue.RemoveAt(0);
            }
            else
            {
                obj = AddToPool();
            }
        }
        if (obj.activeSelf != setActive)
        {
            obj.SetActive(setActive);
        }
        m_inUseQue.Add(obj);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        m_inUseQue.Remove(obj);
        obj.transform.parent = m_transform;
        obj.SetActive(false);
    }

    private GameObject AddToPool()
    {
        GameObject obj = Instantiate(m_prefab, m_transform);
        obj.SetActive(false);
        obj.name += m_objectPool.Count();
        m_objectPool.Add(obj);
        return obj;
    }

}
