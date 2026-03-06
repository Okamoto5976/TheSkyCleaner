using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotHandler : MonoBehaviour
{
    [SerializeField] private BulletPoolManager m_bulletPool;
    [SerializeField] private FloatContainer m_playerShootDelay;
    [SerializeField] private AxisVector3Container m_target;
    [SerializeField] private FloatContainer m_shotVelocity;
    [SerializeField] private ReticleController m_reticleController;
    [SerializeField] private float m_shotDepthOffset;

    [Header("Audio")]
    [SerializeField] private AudioContainer m_shotSound;
    [SerializeField] private AudioHandler m_audioSource;

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
        m_audioSource.PlayOneShot(m_shotSound);
    }

    public void ShootBullet()
    {
        IDamage target = m_reticleController.GetPrimaryTarget();
        Debug.Log(target);
        foreach (var offset in m_shotPositions)
        {
            BulletController bulletController = m_bulletPool.GetComponentFromPool();
            Vector3 pos = m_transform.position + offset.Value;
            Vector3 reticlePos = m_target.Value;
            reticlePos.z += m_shotDepthOffset;
            Vector3 dir = (reticlePos - pos).normalized;

            bulletController.InjectTarget(target);
            bulletController.InjectDirection(dir);
            bulletController.InjectVelocity(m_shotVelocity.Value);
            bulletController.Initialize(pos);
            bulletController.gameObject.SetActive(true);
        }
    }
}
