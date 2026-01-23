using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    struct ButtonEvent
    {
        [SerializeField] private TriggerContainer inputTriggerContainer;
        [SerializeField] private BooleanContainer inputHoldStateContainer;

        [SerializeField] private TriggerContainer playerOnTriggerContainer;
        [SerializeField] private BooleanContainer playerHoldStateContainer;
        public readonly TriggerContainer InputTriggerContainer => inputTriggerContainer;
        public readonly BooleanContainer InputHoldStateContainer => inputHoldStateContainer;
        public readonly TriggerContainer PlayerOnTriggerContainer => playerOnTriggerContainer;
        public readonly BooleanContainer PlayerHoldStateContainer => playerHoldStateContainer;
    }


    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Events")]
    //[SerializeField] private InputContainer m_inputContainer;
    [SerializeField] private AxisVector2Container m_inputMovementAxisContainer;
    [SerializeField] private AxisVector2Container m_inputReticleAxisContainer;
    [SerializeField] private PlayerActionContainer m_playerActionContainer;
    [SerializeField] private UnityEvent<Vector2> m_onMoveAll;
    [SerializeField] private UnityEvent<float> m_onMoveHorizontal;
    [SerializeField] private UnityEvent<float> m_onMoveVertical;
    [SerializeField] private ButtonEvent m_onMainAction;
    [SerializeField] private ButtonEvent m_onSecondaryAction;
    [SerializeField] private UnityEvent<float> m_onChangeSpeed;
    [SerializeField] private UnityEvent<bool> m_onDodge;

    private float m_strongHoldValue;
    private float m_weakHoldValue;

    private void Awake()
    {
        m_strongHoldValue = 0;
        m_weakHoldValue = 0;
    }

    private void OnEnable()
    {
        m_inputMovementAxisContainer.OnValueChanged += SetMovementAxis;
        m_inputReticleAxisContainer.OnValueChanged += SetReticleAxis;
        m_onMainAction.InputTriggerContainer.OnValueChanged += OnMainActionTap;
        m_onMainAction.InputHoldStateContainer.OnValueChanged += SetMainActionHoldState;
    }

    private void OnDisable()
    {
        m_inputMovementAxisContainer.OnValueChanged -= SetMovementAxis;
        m_inputReticleAxisContainer.OnValueChanged -= SetReticleAxis;
        m_onMainAction.InputTriggerContainer.OnValueChanged -= OnMainActionTap;
        m_onMainAction.InputHoldStateContainer.OnValueChanged -= SetMainActionHoldState;
    }

    private void Update()
    {
        ChangeSpeed(m_strongHoldValue + m_weakHoldValue);
    }

    public void SetMovementAxis(Vector2 axis) => m_playerActionContainer.SetMovementAxis(axis);
    public void SetReticleAxis(Vector2 axis) => m_playerActionContainer.SetReticleAxis(axis);

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

    public void OnMainActionTap()
    {
        m_onMainAction.PlayerOnTriggerContainer.Trigger();
    }
    public void SetMainActionHoldState(bool state)
    {
        ActionsHoldState(state, m_onMainAction);
    }

    public void SecondaryActionTap()
    {
        m_onSecondaryAction.PlayerOnTriggerContainer.Trigger();
    }

    public void SecondaryActionHoldSetState(bool state)
    {
        ActionsHoldState(state, m_onSecondaryAction);
    }


    private void ActionsHoldState(bool state, ButtonEvent buttonEvent)
    {
        m_logger.Log(state ? "Hold Started" : "Hold Cancelled", this);
        buttonEvent.PlayerHoldStateContainer.Value = state;
    }
}
