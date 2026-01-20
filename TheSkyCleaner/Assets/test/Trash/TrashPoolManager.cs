using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

public class TrashPoolManager : MonoBehaviour
{
    [SerializeField] public GameObject m_trashSpawnPrefab;
    [SerializeField] private int m_poolCount;
    [SerializeField] private bool m_forcePoolCount = true;

    [Header("X/Y のランダム生成範囲")]
    [SerializeField] private Vector3 m_spawnPos;      // 生成位置（Zのみ使用）
    [SerializeField] private float m_spawnXMin;       // X 最小
    [SerializeField] private float m_spawnXMax;       // X 最大
    [SerializeField] private float m_spawnYMin;       // Y 最小
    [SerializeField] private float m_spawnYMax;       // Y 最大
    [SerializeField] public int m_setActiveFalsePosZ;        //debug用
    [Header("生成間間隔")]
    [SerializeField] private float m_spawnInterval;    // 生成間隔

    private List<TEManager> m_trashPool;
    private List<TEManager> m_inUseQue;

    private Transform m_transform;

    private void Awake()
    {
        m_trashPool = new();
        m_inUseQue = new();
        m_transform = transform;
        for (int i = 0; i < m_poolCount; i++)
        {
            AddToPool();
        }

        StartCoroutine("TrashCount");
    }

    public TEManager GetFromPool(bool setActive = false)
    {
        TEManager obj = m_trashPool.FirstOrDefault(x => !m_inUseQue.Contains(x));
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
        AdjustPosition(obj);
        if (obj.gameObject.activeSelf != setActive)
        {
            obj.gameObject.SetActive(setActive);
        }
        m_inUseQue.Add(obj);
        return obj;
    }

    public void ReturnToPool(TEManager obj)
    {
        m_inUseQue.Remove(obj);
        obj.transform.parent = m_transform;
        obj.gameObject.SetActive(false);
    }

    private TEManager AddToPool()
    {
        GameObject obj = Instantiate(m_trashSpawnPrefab, m_transform);
        obj.GetComponent<ReturnTrashtToPool>().InjectPoolManager(this);
        TEManager teManager = obj.GetComponent<TEManager>();
        obj.SetActive(false);
        obj.name += m_trashPool.Count();
        m_trashPool.Add(teManager);
        return teManager;
    }

    private void AdjustPosition(TEManager obj)
    {
        // X/Y を指定範囲でランダム、Z は既存の m_spawnPos.z を採用
        float randX = UnityEngine.Random.Range(m_spawnXMin, m_spawnXMax);
        float randY = UnityEngine.Random.Range(m_spawnYMin, m_spawnYMax);
        obj.transform.position = new Vector3(randX, randY, m_spawnPos.z);
    }

    private IEnumerator TrashCount()
    {
        while(true)
        {
            yield return new WaitForSeconds(m_spawnInterval);
            TEManager manager = GetFromPool(true);
            manager.InitializeTE(false);
            Debug.Log(manager.CurrentType);
        }
    }

}
