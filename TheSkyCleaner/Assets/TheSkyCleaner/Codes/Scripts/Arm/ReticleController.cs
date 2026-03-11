using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    //[SerializeField] private ActivateObjectAtPosition m_activateOb;

    [SerializeField] private Canvas m_canvas;
    [SerializeField] private RectTransform m_canvasSize;

    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private EnemyPoolManager m_enemypool;
    [SerializeField] private TrashPoolManager m_trashpool;
    [SerializeField] private LargeTrashPoolManager m_largeTrashpool;
    [SerializeField] private HumanPoolManager m_humanPool;


    [SerializeField] private Image m_lockOnMarkerPrefab;
    [SerializeField] private Image m_shotMarkerPrefab;

    [SerializeField] private RectTransform m_rect;
    [SerializeField] private AxisVector3Container m_targetAxis;
    [SerializeField] private float m_reticleSpeed;
    private float m_reticleDistance;

    [SerializeField] private BossController m_bossController;
    [SerializeField] private BooleanContainer m_isBossActive;

    [SerializeField] private IntegerContainer m_maxArmCount;

    [SerializeField] private float m_maxDistance = 30;

    public Vector3 RectPos { get => m_rect.position; }
    public int MaxCount { get => m_maxArmCount.Value; }

    [SerializeField] private AxisVector3Container m_playerAxis;
    private Vector3 m_playerPos;
    private Vector3 m_currentPos;

    public Vector3 PlayerPos { get => m_playerPos; }

    private List<ILockOnTarget> m_LockOnCandidates = new List<ILockOnTarget>();
    private List<ILockOnTarget> m_LockTargets = new List<ILockOnTarget>();
    private List<ILockOnTarget> m_SaveTargets = new List<ILockOnTarget>();
    private List<Image> m_lockOnMarkers = new List<Image>();

    private Image m_shotMarker;

    [SerializeField] private FloatContainer m_reticleControll;

    /// <summary>
    /// All targets of ILockOnTarget
    /// </summary>
    public List<ILockOnTarget> LockOnCandidates { get => m_LockOnCandidates; }
    /// <summary>
    /// List of Lockable targets in reticle range
    /// </summary>
    public List<ILockOnTarget> LockTargets { get => m_LockTargets; }
    /// <summary>
    /// Locked on targets
    /// </summary>
    public List<ILockOnTarget> SaveTargets { get => m_SaveTargets; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;

        m_reticleDistance = m_rect.transform.position.z;
    }

    private void Start()
    {
        for (int i = 0; i < m_maxArmCount.Value; i++)
        {
            Image marker = Instantiate(m_lockOnMarkerPrefab, m_canvas.transform);
            marker.gameObject.SetActive(false);
            m_lockOnMarkers.Add(marker);
        }

        m_shotMarker = Instantiate(m_shotMarkerPrefab,m_canvas.transform);
        m_shotMarker.gameObject.SetActive(false);
    }

    private void Update()
    {
        m_playerPos = m_playerAxis.Value;

        Vector3 current_pos = m_playerPos;
        Vector3 delta = current_pos - m_currentPos;
        Vector3 pos = m_rect.transform.position;

        pos += delta * m_reticleSpeed;

        Vector3 screenPos = m_mainCamera.WorldToScreenPoint(pos);
        float distance = screenPos.z;

        screenPos.x = Mathf.Clamp(screenPos.x,
            0,
            Screen.width);

        screenPos.y = Mathf.Clamp(screenPos.y,
            0,
            Screen.height);

        screenPos.z = distance;

        pos = m_mainCamera.ScreenToWorldPoint(screenPos);

        m_rect.transform.position = pos;
        m_currentPos = current_pos;

        RemoveSaveEnemies();

        UpdateLockOnMarkers(m_SaveTargets);
        UpdateShotReticle();

        m_targetAxis.SetValue(m_rect.position);

    }

    public void MoveReticle(Vector2 delta)
    {
        Vector3 pos = m_rect.position;
        pos.x += delta.x / 25 * m_reticleControll.Value;
        pos.y += delta.y / 25 * m_reticleControll.Value;
        m_rect.position = pos;
        UpdateLockOnCandidates();
        UpdateLockEnemies();

        UpdateLockOnMarkers(m_SaveTargets);
        UpdateShotReticle();
    }

    public Rect GetScreenRect(RectTransform reticle)
    {
        Camera cam = m_mainCamera;

        var corners = new Vector3[4];
        reticle.GetWorldCorners(corners);

        corners[0].z = m_reticleDistance;
        Vector3 min = cam.WorldToScreenPoint(corners[0]);

        corners[2].z = m_reticleDistance;
        Vector3 max = cam.WorldToScreenPoint(corners[2]);

        return Rect.MinMaxRect(
            min.x,
            min.y,
            max.x,
            max.y);
    }

    private void UpdateLockOnCandidates()//”ÍˆÍ“à‚Ì‚·‚×‚Ä‚Ìpool“à‚Ì“G‚ðŽæ“¾
    {
        m_LockOnCandidates.Clear();

        var enemies = m_enemypool.GetActiveComponents();
        var trashes = m_trashpool.GetActiveComponents();
        var largeTrashes = m_largeTrashpool.GetActiveComponents();
        var humans = m_humanPool.GetActiveComponents();

        LockOnTargets(enemies);
        LockOnTargets(trashes);
        LockOnTargets(largeTrashes);
        LockOnTargets(humans);

        if (m_isBossActive.Value)
        {
            ILockOnTarget ilot = m_bossController;
            Vector3 sp = m_mainCamera.WorldToScreenPoint(ilot.ReticlePosition);

            if (GetScreenRect(m_rect).Contains(new Vector2(sp.x, sp.y)))
                m_LockOnCandidates.Add(m_bossController);
        }
    }

    private void LockOnTargets(IEnumerable<ILockOnTarget> targets)
    {
        Rect lockOnRect = GetScreenRect(m_rect);

        foreach (var target in targets)
        {
            if (!target.GameObject.activeSelf) continue;

            Vector3 sp = m_mainCamera.WorldToScreenPoint(target.ReticlePosition);

            if (sp.z < m_playerPos.z) continue;
            if (sp.z > m_maxDistance) continue;

            if (lockOnRect.Contains(new Vector2(sp.x, sp.y)))
                m_LockOnCandidates.Add(target);
        }

    }
    private void UpdateLockEnemies()//ŒŸ’m‚³‚ê‚½’†‚Å‹ß‚¢‚à‚Ì‚ð“ü‚ê‚é
    {
        m_LockTargets.Clear();

        Vector3 selfPos = transform.position;

        var sort = m_LockOnCandidates
            .OrderBy(e => (e.ReticlePosition - selfPos).magnitude)
            .Take(m_maxArmCount.Value);

        foreach (var enemy in sort)
        {
            m_LockTargets.Add(enemy);
        }

        for (int i = 0; i < m_LockTargets.Count; i++)
        {
            if (m_LockTargets[i] == null) continue;
            var enemy = m_LockTargets[i];

            if (m_SaveTargets.Contains(enemy))
            {
                m_SaveTargets.Remove(enemy);
                m_SaveTargets.Add(enemy);
                continue;
            }

            if (m_SaveTargets.Count < m_maxArmCount.Value)
            {
                m_SaveTargets.Add(enemy);
            }
            else
            {
                m_SaveTargets.RemoveAt(0);
                m_SaveTargets.Add(enemy);
            }
        }
    }

    private void UpdateLockOnMarkers(List<ILockOnTarget> saveEnemies)
    {
        for (int i = 0; i < m_lockOnMarkers.Count; i++)
        {
            if (i >= saveEnemies.Count)
            {
                m_lockOnMarkers[i].gameObject.SetActive(false);
                continue;
            }

            var enemy = saveEnemies[i];
            var marker = m_lockOnMarkers[i];

            marker.transform.position = enemy.ReticlePosition;

            marker.gameObject.SetActive(true);
        }
    }

    private void RemoveSaveEnemies()
    {
        for (int i = m_SaveTargets.Count - 1; i >= 0; i--)
        {
            var enemy = m_SaveTargets[i];
            float reticleDistance = Vector3.Distance(m_mainCamera.transform.position, m_rect.position);

            Vector3 pos = m_mainCamera.WorldToViewportPoint(enemy.ReticlePosition);

            if (pos.z < reticleDistance 
                || !enemy.GameObject.activeSelf 
                || enemy == null)
            {
                m_SaveTargets.RemoveAt(i);
            }
        }
    }

    private void UpdateShotReticle()
    {
        IDamage enemy = GetPrimaryTarget();
        if (enemy == null)
        {
            if (m_shotMarker.gameObject.activeSelf) m_shotMarker.gameObject.SetActive(false);
        }
        else
        {
            if (!m_shotMarker.gameObject.activeSelf) m_shotMarker.gameObject.SetActive(true);

            m_shotMarker.transform.position = enemy.ReticlePosition;
        }
    }

    public IDamage GetPrimaryTarget()
    {
        if (m_LockTargets
            .FirstOrDefault(x => x is IDamage) is not IDamage sortEnemies) return null;
        return sortEnemies;
    }
}
