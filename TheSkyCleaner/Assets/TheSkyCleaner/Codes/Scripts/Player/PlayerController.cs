using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Components")]
    [SerializeField] private TiltHandler m_playerTiltHandler;
    [SerializeField] private ArmController m_armController;
    [SerializeField] private AnimatorVariableDriver m_animatorVariableDriver;

    [SerializeField] private StringContainer m_dodgeAnimationToggleBoolName;
    [SerializeField] private StringContainer m_dodgeAnimationHorizontalFloatName;

    [Header("Global Variable Containers")]
    [SerializeField] private PlayerStatus m_playerStatus;

    [Header("Events")]
    [SerializeField] private InputContainer m_inputContainer;
    [SerializeField] private UnityEvent<float> m_onChangeSpeed;

    private MovementHandler m_movementHandler;
    private PlayerAttackController m_playerAttackController;
    private float m_strongHoldValue;
    private float m_weakHoldValue;
    private Vector2 m_movementAxis;
    private Vector2 m_reticleAxis;

    private Transform m_transform;

    private void Awake()
    {
        m_transform = transform;
        m_movementHandler = GetComponent<MovementHandler>();
        m_playerAttackController = GetComponent<PlayerAttackController>();
        m_strongHoldValue = 0;
        m_weakHoldValue = 0;
        m_movementAxis = Vector2.zero;
    }

    private void OnEnable()
    {
        m_inputContainer.StrongAction.Tap.OnTrigger += OnPlayerDodge;
        m_inputContainer.StrongAction.HoldState.OnValueChanged += OnStrongHold;
        m_inputContainer.WeakAction.HoldState.OnValueChanged += OnWeakHold;
    }

    private void OnDisable()
    {
        m_inputContainer.StrongAction.Tap.OnTrigger -= OnPlayerDodge;
        m_inputContainer.StrongAction.HoldState.OnValueChanged -= OnStrongHold;
        m_inputContainer.WeakAction.HoldState.OnValueChanged -= OnWeakHold;
    }

    private void Update()
    {
        m_playerStatus.UpdateMovementInput(m_inputContainer.MovementAxis);
        MovePlayer(ref m_movementAxis);
        m_playerTiltHandler.TiltOnYaw(m_movementAxis);
        m_playerTiltHandler.TiltYaw(m_movementAxis.x);
        ChangeSpeed(m_strongHoldValue + m_weakHoldValue);

        PassReticle();
    }

    private void PassReticle()
    {
        m_reticleAxis = m_inputContainer.ReticleAxis;
        m_armController.MoveReticle(m_reticleAxis);
    }

    public void OnPlayerDodge()
    {
        m_animatorVariableDriver.TriggerBool(m_dodgeAnimationToggleBoolName.Value);
    }

    public void MovePlayer(ref Vector2 axis)
    {
        axis = m_inputContainer.MovementAxis;
        m_movementHandler.MoveOnZ(axis);
        m_playerStatus.UpdateGlobalPosition(m_transform.position);
        m_animatorVariableDriver.Drive(m_dodgeAnimationHorizontalFloatName.Value, axis.x);
    }

    public void OnStrongHold(bool state)
    {
        m_strongHoldValue = state ? 1 : 0;
    }
    public void OnWeakHold(bool state)
    {
        m_weakHoldValue = state ? -1 : 0;
    }
    public void ChangeSpeed(float dir)
    {
        m_onChangeSpeed.Invoke(dir);
    }
}
