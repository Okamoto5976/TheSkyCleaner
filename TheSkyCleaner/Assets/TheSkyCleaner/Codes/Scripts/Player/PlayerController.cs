using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    struct ButtonEvent
    {
        [SerializeField] UnityEvent m_tap;
        [SerializeField] UnityEvent m_holdStarted;
        [SerializeField] UnityEvent m_holdCancelled;
        public readonly UnityEvent Tap => m_tap;
        public readonly UnityEvent HoldStarted => m_holdStarted;
        public readonly UnityEvent HoldCancelled => m_holdCancelled;
    }


    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Components")]
    [SerializeField] private TiltHandler m_playerTiltHandler;
    [SerializeField] private ArmController m_armController;
    [SerializeField] private AnimatorVariableDriver m_animatorVariableDriver;

    [SerializeField] private StringContainer m_dodgeAnimationToggleBoolName;

    [Header("Global Variable Containers")]
    [SerializeField] private AxisVector3Container m_playerPositionContainer;

    [Header("Events")]
    [SerializeField] private InputContainer m_inputContainer;
    [SerializeField] private UnityEvent<Vector2> m_onMoveAll;
    [SerializeField] private UnityEvent<Vector2> m_onReticle;
    [SerializeField] private UnityEvent<float> m_onMoveHorizontal;
    [SerializeField] private UnityEvent<float> m_onMoveVertical;
    [SerializeField] private ButtonEvent m_onMainAction;
    [SerializeField] private ButtonEvent m_onSecondaryAction;
    [SerializeField] private UnityEvent<float> m_onChangeSpeed;
    [SerializeField] private UnityEvent<bool> m_onDodge;

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
    }

    private void OnDisable()
    {
        m_inputContainer.StrongAction.Tap.OnTrigger -= OnPlayerDodge;
    }

    private void Update()
    {
        MovePlayer(ref m_movementAxis);
        m_onMoveAll.Invoke(m_movementAxis);
        m_playerTiltHandler.TiltOnYaw(m_movementAxis);
        m_playerTiltHandler.TiltYaw(m_movementAxis.x);
        m_onMoveHorizontal.Invoke(m_movementAxis.x);
        m_onMoveVertical.Invoke(m_movementAxis.y);
        ChangeSpeed(m_strongHoldValue + m_weakHoldValue);

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
        m_playerPositionContainer.SetValue(m_transform.position);
    }


    public void MoveAll(Vector2 dir)
    {
        m_movementAxis = dir;
        m_onMoveAll.Invoke(dir);
        m_onMoveHorizontal.Invoke(dir.x);
        m_onMoveVertical.Invoke(dir.y);
    }

    public void ReticleAll(Vector2 dir)
    {
        m_onReticle.Invoke(dir);
    }

    public void OnStrongHold(bool state)
    {
        m_strongHoldValue = state ? 1 : 0;
    }
    public void OnStrongAction(bool state)
    {
        m_onDodge.Invoke(state);
    }
    public void OnWeakHold(bool state)
    {
        m_weakHoldValue = state ? -1 : 0;
    }
    public void ChangeSpeed(float dir)
    {
        m_onChangeSpeed.Invoke(dir);
    }

    public void MainActionTap()
    {
        m_onMainAction.Tap.Invoke();
    }
    public void MainActionHoldSetState(bool state)
    {
        ActionsHoldState(state, m_onMainAction);
    }

    public void SecondaryActionTap()
    {
        m_onSecondaryAction.Tap.Invoke();
    }

    public void SecondaryActionHoldSetState(bool state)
    {
        ActionsHoldState(state, m_onSecondaryAction);
    }


    private void ActionsHoldState(bool state, ButtonEvent buttonEvent)
    {
        if (state)
        {
            m_logger.Log("Hold Started", this);
            buttonEvent.HoldStarted.Invoke();
        }
        else
        {
            m_logger.Log("Hold Cancelled", this);
            buttonEvent.HoldCancelled.Invoke();
        }
    }
}
