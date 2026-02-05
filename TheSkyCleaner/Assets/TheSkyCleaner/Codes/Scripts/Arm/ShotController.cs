using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField] private ReticleController m_reticleController;

    [SerializeField] private AxisVector3Container m_targetAxis;


    private Vector3 m_rect;

    private void Update()
    {
        m_rect = m_reticleController.RectPos;

        var enemypos = Sorted(m_rect);


        m_targetAxis.SetValue(enemypos);
    }

    private Vector3 Sorted(Vector3 defaultPos)
    {
        var sortEnemies = m_reticleController.LockEnemies;

        foreach (var enemy in sortEnemies)
        {
            if (enemy is IDamage)
            {
                return enemy.Transform.position;
            }
        }

        return defaultPos;
    }
}
