using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ReticleController : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private RectTransform m_canvasSize;

    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private EnemyPoolManager m_enemypoolmanager;
    [SerializeField] private CollectPoolManager m_collectpoolmanager;


    [SerializeField] private Image m_lockOnMarkerPrefab;

    [SerializeField] private RectTransform m_rect;
    [SerializeField] private float m_reticleSpeed;
    [SerializeField] private float m_reticleDistance;

    [SerializeField] private int m_maxCount;

    public int MaxCount { get => m_maxCount; }

    [SerializeField] private Transform m_player;
    private Vector3 m_playerPos;

    private List<ILockOnTarget> m_LockOnCandidates = new List<ILockOnTarget>();
    private List<ILockOnTarget> m_LockEnemies = new List<ILockOnTarget>();
    private List<ILockOnTarget> m_SaveEnemies = new List<ILockOnTarget>();
    private List<Image> m_lockOnMarkers = new List<Image>();

    public List<ILockOnTarget> SaveEnemies { get => m_SaveEnemies; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {

        for (int i = 0; i < m_maxCount; i++)
        {
            Image marker = Instantiate(m_lockOnMarkerPrefab, m_canvas.transform);
            marker.gameObject.SetActive(false);
            m_lockOnMarkers.Add(marker);
        }
    }

    private void Update()
    {
        Vector3 current_pos = m_player.position;
        Vector3 delta = current_pos - m_playerPos;
        Vector3 pos = m_rect.transform.position;

        pos += delta * m_reticleSpeed;

        Vector3 camPos = m_mainCamera.transform.position;
        float halfH = m_mainCamera.orthographicSize;
        float halfW = halfH * m_mainCamera.aspect;

        pos.x = Mathf.Clamp(pos.x,
            camPos.x - halfW,
            camPos.x + halfW);

        pos.y = Mathf.Clamp(pos.y,
            camPos.y - halfH,
            camPos.y + halfH);

        pos.z = m_reticleDistance;

        m_rect.transform.position = pos;
        m_playerPos = current_pos;

        RemoveSaveEnemies();
    }

    public void MoveReticle(Vector2 delta)
    {
        Vector3 pos = m_rect.position;
        pos.x += delta.x / 50;
        pos.y += delta.y / 50;
        m_rect.position = pos;
        UpdateLockOnCandidates();
        UpdateLockEnemies();
        UpdateLockOnMarkers(m_SaveEnemies);
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

    private void UpdateLockOnCandidates()//範囲内のすべてのpool内の敵を取得
    {
        m_LockOnCandidates.Clear();

        Camera cam = m_mainCamera;

        Rect lockOnRect = GetScreenRect(m_rect);

        var enemies = m_enemypoolmanager.GetActiveComponents();
        var collects = m_collectpoolmanager.GetActiveComponents();

        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeSelf == true) continue;

            Vector3 vp = cam.WorldToScreenPoint(enemy.transform.position);

            float reticleDistance = Vector3.Distance(cam.transform.position, m_rect.position);


            Vector3 sp = m_mainCamera.WorldToScreenPoint(enemy.transform.position);
            Vector2 enemyScreenPos = new Vector2(sp.x, sp.y);

            //Debug.Log($"スクリーン{enemyScreenPos}");


            if (sp.z < m_player.position.z) continue;

            if (lockOnRect.Contains(enemyScreenPos))
                m_LockOnCandidates.Add(enemy);
        }

        foreach (var collect in collects)
        {
            if (!collect.gameObject.activeSelf == true) continue;

            Vector3 vp = cam.WorldToScreenPoint(collect.transform.position);

            float reticleDistance = Vector3.Distance(cam.transform.position, m_rect.position);


            Vector3 sp = m_mainCamera.WorldToScreenPoint(collect.transform.position);
            Vector2 collectScreenPos = new Vector2(sp.x, sp.y);

            //Debug.Log($"スクリーン{enemyScreenPos}");


            if (sp.z < reticleDistance) continue;// カメラよりも後ろ？

            if (lockOnRect.Contains(collectScreenPos))
                m_LockOnCandidates.Add(collect);
        }
    }

    private void UpdateLockEnemies()//検知された中で近いものを入れる
    {
        m_LockEnemies.Clear();

        Vector3 selfPos = transform.position;

        var sort = m_LockOnCandidates
            .OrderBy(e => (e.Transform.position - selfPos).magnitude)
            .Take(m_maxCount);

        foreach (var enemy in sort)
        {
            m_LockEnemies.Add(enemy);
        }

        for (int i = 0; i < m_LockEnemies.Count; i++)
        {
            if (m_LockEnemies[i] == null) continue;
            var enemy = m_LockEnemies[i];

            if (m_SaveEnemies.Contains(enemy))
            {
                m_SaveEnemies.Remove(enemy);
                m_SaveEnemies.Add(enemy);
                continue;
            }

            if (m_SaveEnemies.Count < m_maxCount)
            {
                m_SaveEnemies.Add(enemy);
            }
            else
            {
                m_SaveEnemies.RemoveAt(0);
                m_SaveEnemies.Add(enemy);
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

            marker.transform.position = enemy.Transform.position;

            marker.gameObject.SetActive(true);
        }
    }

    private void RemoveSaveEnemies()
    {
        for (int i = m_SaveEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = m_SaveEnemies[i];
            float reticleDistance = Vector3.Distance(m_mainCamera.transform.position, m_rect.position);

            Vector3 pos = m_mainCamera.WorldToViewportPoint(enemy.Transform.position);

            if (pos.z < reticleDistance)
            {
                m_SaveEnemies.RemoveAt(i);
            }
        }
    }
}
