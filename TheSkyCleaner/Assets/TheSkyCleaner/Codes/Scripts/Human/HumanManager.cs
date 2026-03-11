using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HumanPoolManager))]
public class HumanManager : MonoBehaviour
{
    [SerializeField] private Bounds m_spawnArea;
    [SerializeField] private float m_minimumSpawnInterval;
    [SerializeField] private float m_maximumSpawnInterval;


    private HumanPoolManager m_humanPool;
    private WaitForSeconds m_sleep;
    private void Awake()
    {
        m_humanPool = GetComponent<HumanPoolManager>();
        StartCoroutine(SpawnHumanOnInterval());
    }

    private void SpawnHuman()
    {
        HumanController h = m_humanPool.GetComponentFromPool();
        h.Transform.position = new Vector3(
            Random.Range(-m_spawnArea.extents.x, m_spawnArea.extents.x),
            Random.Range(-m_spawnArea.extents.y, m_spawnArea.extents.y),
            Random.Range(-m_spawnArea.extents.z, m_spawnArea.extents.z)
            ) + m_spawnArea.center;
        h.GameObject.SetActive(true);
    }

    private IEnumerator SpawnHumanOnInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_minimumSpawnInterval, m_maximumSpawnInterval));
            SpawnHuman();
        }
    }
}
