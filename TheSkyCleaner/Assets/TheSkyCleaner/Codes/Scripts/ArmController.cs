using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ArmController : MonoBehaviour
{
    [SerializeField] private RectTransform m_rect;
    [SerializeField] private Canvas m_canvas;

    private List<T_Enemy> m_LockOnCandidates = new List<T_Enemy>();
    private List<T_Enemy> m_LockEnemies = new List<T_Enemy>();
    private List<Image> m_lockOnMarkers = new List<Image>();
    //private List<Transform> m_lockOnArm = new List<Transform>();

    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private EnemyPoolManager m_enemypoolmanager;

    [SerializeField] private List<Arm> m_arms;
    [SerializeField] private List<bool> m_isArmsShot = new List<bool>();
    [SerializeField] private List<Arm> m_activearms = new List<Arm>();
    [SerializeField] private Image m_lockOnMarkerPrefab;
    [SerializeField] private int m_maxCount = 2;

    [SerializeField] private float m_speed = 6.0f;

    public bool m_canmove = true;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        for (int i = 0; i < m_arms.Count; i++)
        {
            Image marker = Instantiate(m_lockOnMarkerPrefab, m_canvas.transform);
            marker.gameObject.SetActive(false);
            m_lockOnMarkers.Add(marker);

            m_arms[i].gameObject.SetActive(true);
            m_isArmsShot.Add(false);
            //m_lockOnArm.Add(m_arms[i].transform);
        }
    }

    private void Update()
    {
        
    }

    public void ArmShot()
    {
        for (int i = 0; i < m_LockEnemies.Count; i++)
        {
            if (m_isArmsShot[i] == false)
            {
                int Id = m_LockEnemies[i].enemyId; //“G‚©ƒSƒ~‚©

                m_arms[i].MoveToEnemy(m_LockEnemies[i].transform, m_speed ,i);
                

                Debug.Log(Id);
                m_isArmsShot[i] = true;
            }
        }
    }

    public void MoveReticle(Vector2 delta)
    {
        Vector2 pos = m_rect.position;
        pos += delta;
        m_rect.position = pos;
        UpdateLockOnCandidates();
        UpdateLockEnemies();
        UpdateLockOnMarkers(m_LockEnemies);
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

    void UpdateLockOnCandidates()//”ÍˆÍ“à‚Ì‚·‚×‚Ä‚Ìpool“à‚Ì“G‚ðŽæ“¾
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

        var sorted = m_LockOnCandidates
            .OrderBy(e => (e.transform.position - selfPos).magnitude)
            .Take(m_lockOnMarkers.Count);

        foreach (var enemy in sorted)
        {
            m_LockEnemies.Add(enemy);
        }
    }

    private void UpdateLockOnMarkers(List<T_Enemy> lockEnemies)
    {
        for (int i = 0; i < m_lockOnMarkers.Count; i++)
        {
            if (i >= lockEnemies.Count)
            {
                m_lockOnMarkers[i].gameObject.SetActive(false);
                continue;
            }

            var enemy = lockEnemies[i];
            var marker = m_lockOnMarkers[i];

            Vector3 screenPos =
            m_mainCamera.WorldToScreenPoint(enemy.transform.position);

            marker.rectTransform.position = screenPos;
            marker.gameObject.SetActive(true);
        }
    }

    public void Return(int index,bool isreturn)
    {
        m_isArmsShot[index] = isreturn;
    }
}
