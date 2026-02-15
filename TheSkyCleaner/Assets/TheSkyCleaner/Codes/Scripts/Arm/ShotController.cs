using UnityEngine;
using System.Collections.Generic;

public class ShotController : MonoBehaviour
{
    [SerializeField] private ReticleController m_reticleController;

    [SerializeField] private AxisVector3Container m_targetAxis;

    [SerializeField] private ObjectPoolManager m_bulletpool;

    private Vector3 m_rect;

    private void Update()
    {
        m_rect = m_reticleController.RectPos;

        var enemypos = Sorted(m_rect);


        m_targetAxis.SetValue(enemypos);
    }

    private Vector3 Sorted(Vector3 defaultPos)
    {
        var sortEnemies = m_reticleController.LockOnCandidates;

        foreach (var enemy in sortEnemies)
        {
            if (enemy is IDamage)
            {
                return enemy.Transform.position;　//m_targetAxisにポジションを入れる
            }
        }

        return defaultPos;
    }
}
