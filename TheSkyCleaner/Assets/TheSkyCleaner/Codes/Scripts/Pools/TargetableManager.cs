using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetableManager : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager m_enemyPoolManager;
    [SerializeField] private TrashPoolManager m_trashPoolManager;
    [SerializeField] private LargeTrashPoolManager m_largeTrashPoolManager;

    private Camera m_mainCamera;

    private void Awake()
    {
        m_mainCamera = Camera.main;
    }

    private IEnumerable<ILockOnTarget> ActiveEnemyPool => m_enemyPoolManager.GetActiveComponents();
    private IEnumerable<ILockOnTarget> ActiveTrashPool => m_trashPoolManager.GetActiveComponents();
    private IEnumerable<ILockOnTarget> ActiveLargeTrashPool => m_largeTrashPoolManager.GetActiveComponents();

    public IEnumerable<ILockOnTarget> GetTargetableList(RectTransform rectTransform, float distance = -1)
    {
        Rect rect = GetScreenRect(rectTransform);
        return GetMergedList()
            .Where(x => IsTargetable(x, rect) && IsInRange(x, distance));
    }

    private Rect GetScreenRect(RectTransform rectTransform)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 min = m_mainCamera.WorldToScreenPoint(corners[0]);
        Vector3 max = m_mainCamera.WorldToScreenPoint(corners[2]);

        return Rect.MinMaxRect(
            min.x,
            min.y,
            max.x,
            max.y
        );
    }

    private bool IsTargetable(ILockOnTarget me, Rect area)
    {
        Vector3 screenPos = m_mainCamera.WorldToScreenPoint(me.Transform.position);
        return area.Contains(screenPos);
    }

    private bool IsInRange(ILockOnTarget me, float distance)
    {
        if (distance < 0) return true;
        return me.Transform.position.z < distance;
    }

    private IEnumerable<ILockOnTarget> GetMergedList()
    {
        var en = ActiveEnemyPool
            .Concat(ActiveTrashPool)
            .Concat(ActiveLargeTrashPool);
        return en;
    }
}
