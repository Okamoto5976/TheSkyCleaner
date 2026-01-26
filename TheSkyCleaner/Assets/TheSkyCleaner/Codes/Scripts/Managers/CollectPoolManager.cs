using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectPoolManager : MonoBehaviour
{
    [SerializeField] private T_Collect m_collect;
    [SerializeField] private int m_poolCount;
    [SerializeField] private bool m_forcePoolCount = true;

    private List<T_Collect> m_objectCollect;
    private List<T_Collect> m_inUseQue;

    private Transform m_transform;

    private void Awake()
    {
        m_objectCollect = new();
        m_inUseQue = new();
        m_transform = transform;
        for (int i = 0; i < m_poolCount; i++)
        {
            AddToPool();
        }
    }

    public T_Collect GetFromPool(Vector3 spawnPosition, bool setActive = false)
    {
        T_Collect obj = m_objectCollect.FirstOrDefault(x => !m_inUseQue.Contains(x));
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

        m_collect.transform.position = spawnPosition;

        if (obj.gameObject.activeSelf != setActive)
        {
            Debug.Log(setActive);
            obj.gameObject.SetActive(setActive);
        }
        m_inUseQue.Add(obj);
        return obj;
    }

    public void ReturnToPool(T_Collect obj)
    {
        m_inUseQue.Remove(obj);
        obj.transform.parent = m_transform;
        obj.gameObject.SetActive(false);
    }

    private T_Collect AddToPool()
    {
        T_Collect obj = Instantiate(m_collect, m_transform);
        obj.gameObject.SetActive(false);
        obj.name += m_objectCollect.Count;
        m_objectCollect.Add(obj);
        return obj;
    }

    public IReadOnlyList<T_Collect> GetActiveEnemies()
    {
        return m_inUseQue;
    }
}
