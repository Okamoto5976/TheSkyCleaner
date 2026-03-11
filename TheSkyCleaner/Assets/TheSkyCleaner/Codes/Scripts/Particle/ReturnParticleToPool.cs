using UnityEngine;

public class ReturnParticleToPool : ReturnObjectToPool
{
    [SerializeField] private GameObject[] m_particles;
    private void Update()
    {
        bool isAllDisabled = true;
        foreach (var p in m_particles)
        {
            if (p.activeSelf)
            {
                isAllDisabled = false;
                break;
            }
        }

        if (isAllDisabled)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        foreach (var p in m_particles)
        {
            p.SetActive(true);
        }
    }
    private void OnDisable()
    {
        ReturnToPool();
    }
}
