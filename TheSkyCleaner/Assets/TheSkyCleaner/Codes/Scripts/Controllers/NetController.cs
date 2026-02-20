using UnityEngine;

public class NetController : MonoBehaviour
{
    [SerializeField] private AxisVector3Container m_playerPosition;
    [SerializeField] private AxisVector3Container m_reticlePosition;
    [SerializeField] private Transform m_camera;
    [SerializeField] private BooleanContainer m_isPlayerAlive;
    [SerializeField] private TriggerContainer m_keyInput;
    [SerializeField] private IntegerContainer m_netCount;
    [SerializeField] private GameObject m_netObject;
    [SerializeField] private Transform m_netSpawnTarget;
    [SerializeField] private Transform m_netMoveTarget;

    [SerializeField] private float m_netDistance;
    

    private Transform m_transform;
    private Transform m_netObjectTransform;

    private void Awake()
    {
        m_netObject.SetActive(false);
        m_transform = transform;
        m_netObjectTransform = m_netObject.transform;
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
        Vector3 pos = m_reticlePosition.Value;
        Vector3 movePos = Camera.main.WorldToScreenPoint(pos);
        movePos.z = m_netDistance;
        pos.z = m_netSpawnTarget.position.z;
        m_netSpawnTarget.position = pos;
        Vector3 wp = Camera.main.ScreenToWorldPoint(movePos);
        m_netMoveTarget.position = wp;
        //m_netObjectTransform.rotation = Quaternion.LookRotation((wp - m_camera.position).normalized);

        m_netObject.SetActive(true);
    }
}
