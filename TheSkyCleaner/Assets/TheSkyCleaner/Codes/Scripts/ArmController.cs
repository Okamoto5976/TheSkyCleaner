using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ArmController : MonoBehaviour
{
    [SerializeField] private RectTransform m_rect;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private RectTransform m_canvasSize;
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private EnemyPoolManager m_enemypoolmanager;

    private List<T_Enemy> m_LockOnCandidates = new List<T_Enemy>();
    private List<T_Enemy> m_LockEnemies = new List<T_Enemy>();
    private List<T_Enemy> m_SaveEnemies = new List<T_Enemy>();
    private List<Image> m_lockOnMarkers = new List<Image>();
    [SerializeField] private Image m_lockOnMarkerPrefab;

    [SerializeField] private List<Arm> m_arms;
    private List<bool> m_activeArms = new List<bool>();
    private List<int> m_enemiesId = new List<int>();

    [SerializeField] private int m_maxCount = 2;

    [SerializeField] private float m_speed;
    [SerializeField] private float m_reticleSpeed;

    [SerializeField] private Transform m_plaeyr;
    [SerializeField] private Vector3 m_playerPos;
    private float m_reticleDistance = 5f;

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

            m_arms[i].gameObject.SetActive(true);
            m_activeArms.Add(true);
            //m_lockOnArm.Add(m_arms[i].transform);
        }
    }

    private void Update()
    {
        Vector3 current_pos = m_plaeyr.position;
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
        //// ワールド → Viewport
        //Vector3 vp = m_mainCamera.WorldToViewportPoint(pos);

        //// 画面内に制限
        //vp.x = Mathf.Clamp(vp.x, camPos, 0.95f);
        //vp.y = Mathf.Clamp(vp.y, 0.05f, 0.95f);

        //vp.z = m_reticleDistance;
        // Viewport → ワールド
        //pos = m_mainCamera.ViewportToWorldPoint(vp);



        m_rect.transform.position = pos;
        m_playerPos = current_pos;

    }

    public void ArmShot()
    {
        foreach(var enemies in m_SaveEnemies)
        {
            int id = enemies.GetInstanceID();

            if (m_enemiesId.Contains(id)) continue;

            int index = Getbool();
            if (index == -1) break; //使えるものがない

            //Arm arm = m_arms.FirstOrDefault(a => a.m_canmove);//m_armsから探してないならnull
            //if (arm == null) break;
            m_activeArms[index] = false;

            Arm arm = m_arms[index];

            m_enemiesId.Add(id);

            arm.MoveToEnemy(enemies.transform, m_speed, id, index);
        }
    }

    private int Getbool()
    {
        for (int i = 0; i < m_activeArms.Count; i++)
        {
            if (m_activeArms[i] == true)
                return i;
        }
        return -1;
    }

    public void MoveReticle(Vector2 delta)
    {
        Vector2 pos = m_rect.position;
        pos += delta / 50;
        m_rect.position = pos;
        UpdateLockOnCandidates();
        UpdateLockEnemies();
        UpdateLockOnMarkers(m_SaveEnemies);
    }

    public Rect GetScreenRect(RectTransform reticle)
    {
        Vector3[] corners = new Vector3[4];
        reticle.GetWorldCorners(corners);

        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < 4; i++)
        {
            Vector2 sp = RectTransformUtility.WorldToScreenPoint(
                m_mainCamera,
                corners[i]
            );

            min = Vector2.Min(min, sp);
            max = Vector2.Max(max, sp);
        }

        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);


        //var corners = new Vector3[4];
        //reticle.GetWorldCorners(corners);

        //Vector2 min = RectTransformUtility.WorldToScreenPoint(
        //m_mainCamera, corners[0]); // 左下

        //Vector2 max = RectTransformUtility.WorldToScreenPoint(
        //    m_mainCamera, corners[2]); // 右上

        //return new Rect(
        //    min.x,
        //    min.y,
        //    max.x,
        //    max.y);
    }

    private void UpdateLockOnCandidates()//範囲内のすべてのpool内の敵を取得
    {
        m_LockOnCandidates.Clear();

        Camera cam = m_mainCamera;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        // レティクルのWorld位置(カメラ基準）
        Vector2 reticleWorldPos = new Vector2(
            m_rect.position.x - cam.transform.position.x,
            m_rect.position.y - cam.transform.position.y
        );

        // レティクルの半径（＝判定範囲）
        float reticleRadius = Mathf.Max(
            m_rect.rect.width,
            m_rect.rect.height
        ) * 0.5f * m_rect.lossyScale.x;
        // WorldSpaceUIなのでlossyScale 必須
        //Rect lockOnRect = GetScreenRect(m_rect);

        var enemies = m_enemypoolmanager.GetActiveEnemies();

        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeSelf == true) continue;

        Vector3 vp = cam.WorldToViewportPoint(enemy.transform.position);

        if (vp.z <= 0) continue;

        // Viewport → World（比率補正の核心）
        Vector2 enemyWorldOffset = new Vector2(
            (vp.x - 0.5f) * 2f * halfW,
            (vp.y - 0.5f) * 2f * halfH
        );

        // 距離判定
        if (Vector2.Distance(enemyWorldOffset, reticleWorldPos) <= reticleRadius)
        {
            m_LockOnCandidates.Add(enemy);
        }
            //Vector3 sp = m_mainCamera.WorldToScreenPoint(enemy.transform.position);
            //Vector2 enemyScreenPos = new Vector2(sp.x, sp.y);

            //if (sp.z <= 0) continue;// カメラよりも後ろ？

            //if (lockOnRect.Contains(enemyScreenPos))
            //    m_LockOnCandidates.Add(enemy);
        }
    }

    private void UpdateLockEnemies()//検知された中で近いものを入れる
    {
        m_LockEnemies.Clear();

        Vector3 selfPos = transform.position;

        var sort = m_LockOnCandidates
            .OrderBy(e => (e.transform.position - selfPos).magnitude)
            .Take(m_maxCount);

        foreach (var enemy in sort)
        {
            m_LockEnemies.Add(enemy);
        }

        for(int i = 0;i < m_LockEnemies.Count;i++)
        {
            if (m_LockEnemies[i] == null) continue;
            var enemy = m_LockEnemies[i];

            if (m_SaveEnemies.Contains(enemy))
            {
                m_SaveEnemies.Remove(enemy);
                m_SaveEnemies.Add(enemy);
                continue;
            }

            if(m_SaveEnemies.Count < m_maxCount)
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

    private void UpdateLockOnMarkers(List<T_Enemy> saveEnemies)
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

            Vector3 offset = Vector3.up * 1.5f;

            marker.transform.position = enemy.transform.position + offset;

            //Vector3 screenPos =
            //m_mainCamera.WorldToScreenPoint(enemy.transform.position);

            //marker.rectTransform.position = screenPos;
            marker.gameObject.SetActive(true);
        }
    }

    public void Return(int id,int index)
    {
        m_enemiesId.Remove(id);
        m_activeArms[index] = true;
    }
}
