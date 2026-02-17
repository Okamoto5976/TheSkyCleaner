using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Input")]
    [SerializeField] private InputContainer m_inputContainer;

    [Header("Components")]
    [SerializeField] private TiltHandler m_playerTiltHandler;
    [SerializeField] private ReticleController m_reticleController;
    [SerializeField] private AnimatorVariableDriver m_animatorVariableDriver;

    [SerializeField] private StringContainer m_dodgeAnimationToggleBoolName;
    [SerializeField] private StringContainer m_dodgeAnimationHorizontalFloatName;

    [Header("Global Variable Containers")]
    [SerializeField] private PlayerStatus m_playerStatus;

    [Header("Events")]

    private MovementHandler m_movementHandler;
    private PlayerAttackController m_playerAttackController;
    private Vector2 m_movementAxis;
    private Vector2 m_reticleAxis;

    private Transform m_transform;

    private void Awake()
    {
        m_transform = transform;
        m_movementHandler = GetComponent<MovementHandler>();
        m_playerAttackController = GetComponent<PlayerAttackController>();
        m_movementAxis = Vector2.zero;
    }

    private void OnEnable()
    {
        m_inputContainer.StrongAction.Tap.OnTrigger += OnPlayerDodge;

    }

    private void OnDisable()
    {
        m_inputContainer.StrongAction.Tap.OnTrigger -= OnPlayerDodge;

    }

    private void Update()
    {
        m_playerStatus.UpdateMovementInput(m_inputContainer.MovementAxis);
        PassReticle();
        MovePlayer(ref m_movementAxis);
        m_playerTiltHandler.TiltOnYaw(m_movementAxis);
        m_playerTiltHandler.TiltYaw(m_movementAxis.x);
    }

    private void FixedUpdate()
    {
    }

    private void PassReticle()
    {
        m_reticleAxis = m_inputContainer.ReticleAxis;
        m_reticleController.MoveReticle(m_reticleAxis);
    }

    public void OnPlayerDodge()
    {
        m_animatorVariableDriver.TriggerBool(m_dodgeAnimationToggleBoolName.Value);
    }

    public void MovePlayer(ref Vector2 axis)
    {
        axis = m_inputContainer.MovementAxis;
        m_movementHandler.SetSpeed(m_playerStatus.Speed);
        m_movementHandler.MoveOnZ(axis);
        m_playerStatus.UpdateGlobalPosition(m_transform.position);
        m_animatorVariableDriver.Drive(m_dodgeAnimationHorizontalFloatName.Value, axis.x);
    }


}
