using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private T_Enemy m_enemy;
    [SerializeField] private int m_poolCount;
    [SerializeField] private bool m_forcePoolCount = true;

    private List<T_Enemy> m_objectEnemy;
    private List<T_Enemy> m_inUseQue;

    private Transform m_transform;

    private void Awake()
    {
        m_objectEnemy = new();
        m_inUseQue = new();
        m_transform = transform;
        for (int i = 0; i < m_poolCount; i++)
        {
            AddToPool();
        }
    }

    public T_Enemy GetFromPool(Vector3 spawnPosition,bool setActive = false)
    {
        T_Enemy obj = m_objectEnemy.FirstOrDefault(x => !m_inUseQue.Contains(x));
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

        m_enemy.transform.position = spawnPosition;

        if (obj.gameObject.activeSelf != setActive)
        {
            Debug.Log(setActive);
            obj.gameObject.SetActive(setActive);
        }
        m_inUseQue.Add(obj);
        return obj;
    }

    public void ReturnToPool(T_Enemy obj)
    {
        m_inUseQue.Remove(obj);
        obj.transform.parent = m_transform;
        obj.gameObject.SetActive(false);
    }

    private T_Enemy AddToPool()
    {
        T_Enemy obj = Instantiate(m_enemy, m_transform);
        obj.gameObject.SetActive(false);
        obj.name += m_objectEnemy.Count;
        m_objectEnemy.Add(obj);
        return obj;
    }

    public IReadOnlyList<T_Enemy> GetActiveEnemies()
    {
        return m_inUseQue;
    }
}
