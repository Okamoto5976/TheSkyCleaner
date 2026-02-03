using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [Header("States")]
    [SerializeField] private AxisVector3Container m_globalPosition;
    [SerializeField] private AxisVector3Container m_rotation;
    [SerializeField] private FloatContainer m_speed;
    [SerializeField] private BooleanContainer m_isAlive;
    [SerializeField] private BooleanContainer m_isInvulnerable;

    [Header("Controls")]
    [SerializeField] private AxisVector2Container m_movementAxis;
    [SerializeField] private AxisVector2Container m_rotationAxis;

    public Vector3 GlobalPosition => m_globalPosition.Value;
    public Vector3 Rotation => m_rotation.Value;
    public float Speed => m_speed.Value;
    public bool IsAlive => m_isAlive.Value;
    public bool IsVulnerable => m_isInvulnerable.Value;

    public Vector2 MovementAxis => m_movementAxis.Value;
    public Vector2 RotationAxis => m_rotationAxis.Value;

    public void UpdateGlobalPosition(Vector3 position) => m_globalPosition.SetValue(position);
    public void UpdateRotation(Vector3 rotation) => m_rotation.SetValue(rotation);
    public void UpdateSpeed(float speed) => m_speed.SetValue(speed);
    public void UpdateAliveState(bool isAlive) => m_isAlive.SetValue(isAlive);
    public void UpdateInvulnerabilityState(bool isInvulnerable) => m_isInvulnerable.SetValue(isInvulnerable);
    public void UpdateMovementInput(Vector2 input) => m_movementAxis.SetValue(input);
    public void UpdateRotationInput(Vector2 input) => m_rotationAxis.SetValue(input);
}
