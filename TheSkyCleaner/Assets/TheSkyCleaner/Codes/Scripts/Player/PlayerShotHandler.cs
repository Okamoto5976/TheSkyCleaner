using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotHandler : MonoBehaviour
{
    [SerializeField] private BulletPoolManager m_bulletPool;
    [SerializeField] private FloatContainer m_playerShootDelay;
    [SerializeField] private AxisVector3Container m_target;
    [SerializeField] private FloatContainer m_shotVelocity;

    [SerializeField] private List<AxisVector3Container> m_shotPositions;

    private WaitForSeconds m_waitDelay;
    private Coroutine m_shootDelay;
    private Transform m_transform;

    private void Awake()
    {
        m_waitDelay = new(m_playerShootDelay.Value);
        m_transform = transform;
    }

    public void Shoot()
    {
        m_shootDelay ??= StartCoroutine(InvokeTimer());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator InvokeTimer()
    {
        OnShoot();
        yield return m_waitDelay;
        m_shootDelay = null;
    }

    private void OnShoot()
    {
        ShootBullet();
    }

    public void ShootBullet()
    {
        foreach (var offset in m_shotPositions)
        {
            BulletController bulletController = m_bulletPool.GetComponentFromPool();
            Vector3 pos = m_transform.position + offset.Value;
            bulletController.transform.position = pos;
            Vector3 dir = (m_target.Value - pos).normalized;

            bulletController.InjectDirection(dir);
            bulletController.InjectVelocity(m_shotVelocity.Value);
            bulletController.Initialize();
            bulletController.gameObject.SetActive(true);
        }
    }
}
