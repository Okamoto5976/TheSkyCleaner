using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 m_fov;

    [SerializeField] private float m_lerpTime;
    private Transform m_transform;
    private Camera m_camera;

    private void Awake()
    {
        m_transform = gameObject.transform;
        m_camera = GetComponent<Camera>();
    }

    public void LerpFOV(float dir)
    {
        float target = dir > 0 ? m_fov.z : (dir < 0 ? m_fov.x : m_fov.y); 
        m_camera.fieldOfView = Mathf.LerpAngle(m_camera.fieldOfView, target, Time.deltaTime * m_lerpTime);
    }

}
