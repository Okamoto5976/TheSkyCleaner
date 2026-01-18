using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class ArmController : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Canvas canvas;

    private List<T_Enemy> LockOnCandidates = new List<T_Enemy>();
    private List<T_Enemy> LockEnemies = new List<T_Enemy>();
    private List<Image> lockOnMarkers = new List<Image>();
    private List<Transform> lockOnArm = new List<Transform>();

    [SerializeField] private Camera mainCamera;
    [SerializeField] private EnemyPoolManager m_enemypoolmanager;

    [SerializeField] private Arm[] m_arm;
    [SerializeField] private Image lockOnMarkerPrefab;
    [SerializeField] private int maxLockOnCount = 2;

    [SerializeField] private float m_speed = 6.0f;

    private bool m_canmove = true;

    private void Start()
    {
        for (int i = 0; i < maxLockOnCount; i++)
        {
            Image marker = Instantiate(lockOnMarkerPrefab, canvas.transform);
            marker.gameObject.SetActive(false);
            lockOnMarkers.Add(marker);
            m_arm[i].gameObject.SetActive(true);
            lockOnArm.Add(m_arm[i].transform);
        }
    }

    private void Update()
    {

        rect.position = Mouse.current.position.ReadValue();
        UpdateLockOnCandidates();
        UpdateLockEnemies();
        UpdateLockOnMarkers(LockEnemies);

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (m_canmove == true)
            {
                for (int i = 0; i < LockEnemies.Count; i++)
                {
                    int Id = LockEnemies[i].enemyId;

                    m_arm[i].MoveToEnemy(LockEnemies[i].transform);
                   Debug.Log(Id);
                }

               
                
                //m_canmove = false;
            }
        }
    }

    public Rect GetScreenRect()
    {
        Vector2 size = rect.sizeDelta * canvas.scaleFactor;
        Vector2 pos = rect.position;

        return new Rect(
            pos.x - size.x * 0.5f,
            pos.y - size.y * 0.5f,
            size.x,
            size.y
        );
    }

    void UpdateLockOnCandidates()//”ÍˆÍ“à‚Ì‚·‚×‚Ä‚Ìpool“à‚Ì“G‚ðŽæ“¾
    {
        LockOnCandidates.Clear();

        Rect lockOnRect = GetScreenRect();
        var enemies = m_enemypoolmanager.GetActiveEnemies();

        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeSelf == true) continue;

            Vector3 screenPos =
                mainCamera.WorldToScreenPoint(enemy.transform.position);

            if (screenPos.z <= 0) continue;// ƒJƒƒ‰‚æ‚è‚àŒã‚ëH

            if (lockOnRect.Contains(screenPos))
                LockOnCandidates.Add(enemy);
        }
    }

    private void UpdateLockEnemies()//ŒŸ’m‚³‚ê‚½’†‚Å‹ß‚¢‚à‚Ì‚ð“ü‚ê‚é
    {
        LockEnemies.Clear();

        Vector3 selfPos = transform.position;

        var sorted = LockOnCandidates
            .OrderBy(e => Vector3.SqrMagnitude(e.transform.position - selfPos))
            .Take(maxLockOnCount);

        foreach (var enemy in sorted)
        {
            LockEnemies.Add(enemy);
        }
    }

    private void UpdateLockOnMarkers(List<T_Enemy> lockEnemies)
    {
        for (int i = 0; i < lockOnMarkers.Count; i++)
        {
            if (i >= lockEnemies.Count)
            {
                lockOnMarkers[i].gameObject.SetActive(false);
                continue;
            }

            var enemy = lockEnemies[i];
            var marker = lockOnMarkers[i];

            Vector3 screenPos =
            mainCamera.WorldToScreenPoint(enemy.transform.position);

            marker.rectTransform.position = screenPos;
            marker.gameObject.SetActive(true);
        }
    }
}
