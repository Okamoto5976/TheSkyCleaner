using System.Linq;
using UnityEngine;

public class NetController : MonoBehaviour
{
    [SerializeField] private AxisVector3Container m_playerPosition;
    [SerializeField] private AxisVector3Container m_reticlePosition;
    [SerializeField] private BooleanContainer m_isPlayerAlive;
    [SerializeField] private TriggerContainer m_keyInput;
    [SerializeField] private IntegerContainer m_netCount;
    [SerializeField] private GameObject m_netObject;
    [SerializeField] private Transform m_netSpawnTarget;
    [SerializeField] private Transform m_netMoveTarget;

    [SerializeField] private float m_netDistance;
    [SerializeField] private IntegerContainer m_netDamage;

    [SerializeField] private TargetableManager m_targetableManager;
    [SerializeField] private RectTransform m_reticle;
    [SerializeField] private InventorySO m_inventory;

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioContainer m_netSound;

    private Transform m_transform;
    private Transform m_netObjectTransform;
    private Camera m_mainCamera;
    private Transform m_mainCameraTransform;

    private void Awake()
    {
        m_netObject.SetActive(false);
        m_transform = transform;
        m_netObjectTransform = m_netObject.transform;
        m_mainCamera = Camera.main;
        m_mainCameraTransform = m_mainCamera.transform;
    }


    private void OnEnable()
    {
        m_keyInput.OnTrigger += OnNetShot;
    }

    private void OnDisable()
    {
        m_keyInput.OnTrigger -= OnNetShot;
    }

    public void OnNetShot()
    {
        if (!m_isPlayerAlive.Value) return;
        if (m_netCount.Value <= 0) return;

        m_netObject.SetActive(false);
        m_netCount.SetValue(m_netCount.Value - 1);
        SetNetControlPoints();

        DoNetAction();

        m_audioSource.PlayOneShot(m_netSound.AudioClip, m_netSound.Volume);

        m_netObject.SetActive(true);
    }

    private void DoNetAction()
    {
        Debug.Log("Activate");
        var targets = m_targetableManager.GetTargetableList(m_reticle, m_netDistance).ToList();
        foreach (var target in targets)
        {
            if (target is IDamage damagable)
            {
                if (!damagable.TryCollect(m_netDamage.Value)) continue;
            }
            var drops = target.Collect();
            m_inventory.AddMultiple(drops);
        }
    }

    private void SetNetControlPoints()
    {
        Vector3 pos = m_reticlePosition.Value;
        Vector3 movePos = m_mainCamera.WorldToScreenPoint(pos);
        movePos.z = m_netDistance;
        pos.z = m_netSpawnTarget.position.z;
        m_netSpawnTarget.position = pos;
        Vector3 wp = m_mainCamera.ScreenToWorldPoint(movePos);
        m_netMoveTarget.position = wp;
        //m_netObjectTransform.rotation = Quaternion.LookRotation((wp - m_camera.position).normalized);
    }
}
