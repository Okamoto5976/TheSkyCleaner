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
    [SerializeField] private Vector2 m_playerPos;

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
        Vector2 current_pos = m_plaeyr.position;
        Vector2 delta = current_pos - m_playerPos;

        Vector2 pos = m_rect.position;
        pos += delta * m_reticleSpeed;

        Vector2 canvasSize = m_canvasSize.rect.size;

        pos.x = Mathf.Clamp(pos.x,
            0,
            canvasSize.x);

        pos.y = Mathf.Clamp(pos.y,
            0,
            canvasSize.y);

        m_rect.position = pos;



        m_playerPos = current_pos;

        
    }

    public void ArmShot()
    {
        foreach(var enemies in m_SaveEnemies)
        {
            int id = enemies.GetInstanceID();

            if (m_enemiesId.Contains(id)) continue;

            int index = Getbool();
            if (index == -1) break; //Žg‚¦‚é‚à‚Ì‚ª‚È‚¢

            //Arm arm = m_arms.FirstOrDefault(a => a.m_canmove);//m_arms‚©‚ç’T‚µ‚Ä‚È‚¢‚È‚çnull
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
        pos += delta;
        m_rect.position = pos;
        UpdateLockOnCandidates();
        UpdateLockEnemies();
        UpdateLockOnMarkers(m_SaveEnemies);
    }

    public Rect GetScreenRect()
    {
        Vector2 size = m_rect.sizeDelta * m_canvas.scaleFactor;
        Vector2 pos = m_rect.position;

        return new Rect(
            pos.x - size.x * 0.5f,
            pos.y - size.y * 0.5f,
            size.x,
            size.y
        );
    }

    private void UpdateLockOnCandidates()//”ÍˆÍ“à‚Ì‚·‚×‚Ä‚Ìpool“à‚Ì“G‚ðŽæ“¾
    {
        m_LockOnCandidates.Clear();

        Rect lockOnRect = GetScreenRect();
        var enemies = m_enemypoolmanager.GetActiveEnemies();

        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeSelf == true) continue;

            Vector3 screenPos =
                m_mainCamera.WorldToScreenPoint(enemy.transform.position);

            if (screenPos.z <= 0) continue;// ƒJƒƒ‰‚æ‚è‚àŒã‚ëH

            if (lockOnRect.Contains(screenPos))
                m_LockOnCandidates.Add(enemy);
        }
    }

    private void UpdateLockEnemies()//ŒŸ’m‚³‚ê‚½’†‚Å‹ß‚¢‚à‚Ì‚ð“ü‚ê‚é
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

            Vector3 screenPos =
            m_mainCamera.WorldToScreenPoint(enemy.transform.position);

            marker.rectTransform.position = screenPos;
            marker.gameObject.SetActive(true);
        }
    }

    public void Return(int id,int index)
    {
        m_enemiesId.Remove(id);
        m_activeArms[index] = true;
    }
}
