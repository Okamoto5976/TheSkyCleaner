using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShotController : MonoBehaviour
{
    [SerializeField] private ReticleController m_reticleController;
    [SerializeField] private Camera m_mainCamera;

    [SerializeField] private AxisVector3Container m_targetAxis;

    [SerializeField] private ObjectPoolManager m_bulletpool;

    [SerializeField] private float m_rectzPos;
    private Vector3 m_rect;

    private void Update()
    {
        Vector3 screenPos = 
            m_mainCamera.WorldToScreenPoint(m_reticleController.RectPos);
        screenPos.z = m_rectzPos;
        m_rect = m_mainCamera.ScreenToWorldPoint(screenPos);

        var enemypos = Sorted(m_rect);


        m_targetAxis.SetValue(enemypos);
    }

    private Vector3 Sorted(Vector3 defaultPos)
    {
        ILockOnTarget sortEnemies = m_reticleController.LockOnCandidates
            .FirstOrDefault(x => x is IDamage);

        if (sortEnemies == null) return defaultPos;
        return ((IDamage)sortEnemies).Transform.position;
        //foreach (var enemy in sortEnemies)
        //{
        //    if (enemy is IDamage)
        //    {
        //        return enemy.Transform.position;　//m_targetAxisにポジションを入れる
        //    }
        //}

        //return defaultPos;
    }
}
