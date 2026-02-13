using UnityEngine;

[RequireComponent (typeof(MovementHandler))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Vector3 m_fov;
    [SerializeField] private TiltHandler m_tiltHandler;
    [SerializeField] private TiltHandler m_particleTiltHandler;
    [SerializeField] private PlayerStatus m_playerStatus;
    [SerializeField] private IntegerEventContainer m_playerSpeedAxis;

    [SerializeField] private ParticleSystem[] m_speedUpParticles;

    private Transform m_transform;

    private float m_lerpValue;
    [SerializeField] private float m_lerpTime;
    private float m_cameraSpeed;

    [SerializeField] private bool m_overrideCameraSpeed = false;
    private MovementHandler m_movementHandler;

    private void Awake()
    {
        m_cameraSpeed = 0;
        m_transform = gameObject.transform;
        m_movementHandler = GetComponent<MovementHandler>();
    }

    private void OnEnable()
    {
        m_playerSpeedAxis.OnValueChanged += OnPlayerSpeedAxisChange;
    }

    private void OnDisable()
    {
        m_playerSpeedAxis.OnValueChanged -= OnPlayerSpeedAxisChange;
    }

    private void Update()
    {
        MoveCameraByPlayerInput();
        TiltCameraByPlayerInput();
        UpdateFOV();
    }
    private void UpdateFOV()
    {
        m_lerpValue = Mathf.Lerp(m_lerpValue, m_playerSpeedAxis.Value, Time.deltaTime * m_lerpTime);
        LerpFOV(m_lerpValue);
    }

    private void OnPlayerSpeedAxisChange(int value)
    {
        switch (value)
        {
            case -1:
                SetSpeedUpParticleState(false);
                break;
            case 1:
                SetSpeedUpParticleState(true);
                break;
            default:
            case 0:
                SetSpeedUpParticleState(false);
                break;
        }
    }

    public void SetSpeedUpParticleState(bool state)
    {
        foreach (var particle in m_speedUpParticles)
        {
            if (state)
            {
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
    }

    private void MoveCameraByPlayerInput()
    {
        if (m_playerStatus.MovementAxis != Vector2.zero)
        {
            MoveCamera(m_playerStatus.MovementAxis);
        }
    }

    private void TiltCameraByPlayerInput()
    {
        Vector3 rot = new(m_playerStatus.MovementAxis.y, 0, m_playerStatus.MovementAxis.x);
        TiltCamera(rot);
        rot.y = rot.z;
        rot.z = 0;
        TiltParticle(rot);
    }

    public void MoveCamera(Vector3 dir)
    {
        //m_movementHandler.SetSpeed(m_overrideCameraSpeed ? m_cameraSpeed : m_playerStatus.Speed);
        m_movementHandler.MoveAllGlobal(dir);
    }

    public void SetMoveSpeed(float speed)
    {
        m_overrideCameraSpeed = true;
        m_cameraSpeed = speed;
    }

    public void ReturnCameraSpeed() => m_overrideCameraSpeed = false;

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
