using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Vector3 m_fov;
    [SerializeField] private MovementHandler m_movementHandler;
    [SerializeField] private TiltHandler m_tiltHandler;
    [SerializeField] private TiltHandler m_particleTiltHandler;
    [SerializeField] private AxisVector2Container m_playerMainInputAxis;
    private Transform m_transform;

    private void Awake()
    {
        m_transform = gameObject.transform;
    }

    private void Update()
    {
        MoveCameraByPlayerInput();
        TiltCameraByPlayerInput();
    }

    private void MoveCameraByPlayerInput()
    {
        if (m_playerMainInputAxis.Value != Vector2.zero)
        {
            MoveCamera(m_playerMainInputAxis.Value);
        }
    }

    private void TiltCameraByPlayerInput()
    {
        Vector3 rot = new(m_playerMainInputAxis.Value.y, 0, m_playerMainInputAxis.Value.x);
        TiltCamera(rot);
        rot.y = rot.z;
        rot.z = 0;
        TiltParticle(rot);
    }

    public void MoveCamera(Vector3 dir)
    {
        m_movementHandler.MoveAll(dir);
    }

    public void TiltCamera(Vector3 rot)
    {
        m_tiltHandler.TiltAll(rot);
    }

    public void TiltParticle(Vector3 rot)
    {
        m_particleTiltHandler.TiltAll(rot);
    }

    public void LerpFOV(float dir)
    {
        float newVal;
        if (dir > 0)
        {
            newVal = (float)(dir * (m_fov.z - m_fov.y) + m_fov.y);
        }
        else if (dir < 0)
        {
            newVal = (float)(dir * (m_fov.y - m_fov.x) + m_fov.y);
        }
        else
        {
            newVal = m_fov.y;
        }
        m_camera.fieldOfView = newVal;
    }

}
