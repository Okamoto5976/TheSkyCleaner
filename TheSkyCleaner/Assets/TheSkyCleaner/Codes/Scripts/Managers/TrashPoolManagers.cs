using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TrashPoolManagers : MonoBehaviour
{
    [SerializeField] private T_Trash m_trash;
    [SerializeField] private int m_poolCount;
    [SerializeField] private bool m_forcePoolCount = true;

    private List<T_Trash> m_objectTrash;
    private List<T_Trash> m_inUseQue;

    private Transform m_transform;

    private void Awake()
    {
        m_objectTrash = new();
        m_inUseQue = new();
        m_transform = transform;
        for (int i = 0; i < m_poolCount; i++)
        {
            AddToPool();
        }
    }

    public T_Trash GetFromPool(Vector3 spawnPosition, bool setActive = false)
    {
        T_Trash obj = m_objectTrash.FirstOrDefault(x => !m_inUseQue.Contains(x));
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

        m_trash.transform.position = spawnPosition;

        if (obj.gameObject.activeSelf != setActive)
        {
            Debug.Log(setActive);
            obj.gameObject.SetActive(setActive);
        }
        m_inUseQue.Add(obj);
        return obj;
    }

    public void ReturnToPool(T_Trash obj)
    {
        m_inUseQue.Remove(obj);
        obj.transform.parent = m_transform;
        obj.gameObject.SetActive(false);
    }

    private T_Trash AddToPool()
    {
        T_Trash obj = Instantiate(m_trash, m_transform);
        obj.gameObject.SetActive(false);
        obj.name += m_objectTrash.Count;
        m_objectTrash.Add(obj);
        return obj;
    }

    public IReadOnlyList<T_Trash> GetActiveEnemies()
    {
        return m_inUseQue;
    }
}
