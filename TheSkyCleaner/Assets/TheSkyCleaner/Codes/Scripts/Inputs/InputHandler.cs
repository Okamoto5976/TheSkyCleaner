using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [System.Serializable]
    struct ButtonAction
    {
        [SerializeField] private InputActionReference inputActionReference; 
        public readonly InputAction Action => inputActionReference.action;

        [SerializeField] private UnityEvent onTap;
        public readonly UnityEvent OnTapEvent => onTap;
        [SerializeField] private UnityEvent<float> whileHoldTime;
        public readonly UnityEvent<float> WhileHoldTimeEvent => whileHoldTime;
        [SerializeField] private UnityEvent<bool> onHold;
        public readonly UnityEvent<bool> OnHoldEvent => onHold;

        [HideInInspector] public float time;
        [HideInInspector] public bool state;
    }

    [Header("Logger")]
    [SerializeField] private Logger m_logger;
    [Header("Variable Container")]
    [SerializeField] private InputContainer m_container;

    [Header("Control Events")]
    [SerializeField] private InputActionReference m_movementAxis;
    [SerializeField] private UnityEvent<Vector2> m_onMovementAxis;

    [SerializeField] private InputActionReference m_reticleAxis;
    [SerializeField] private UnityEvent<Vector2> m_onReticleAxis;

    [SerializeField] private ButtonAction m_mainAction;
    [SerializeField] private ButtonAction m_subAction;
    [SerializeField] private ButtonAction m_shoulderLeft;
    [SerializeField] private ButtonAction m_shoulderRight;

    [SerializeField] private InputActionReference m_axialAction;
    [SerializeField] private UnityEvent<float> m_onAxialAction;

    private void Awake()
    {
        m_mainAction.state = false;
        m_subAction.state = false;
    }

    private void Update()
    {
        m_container.SetMovementAxis(m_movementAxis.action.ReadValue<Vector2>());
        m_onMovementAxis.Invoke(m_container.MovementAxis);

        m_container.SetReticleAxis(m_reticleAxis.action.ReadValue<Vector2>());
        m_onReticleAxis.Invoke(m_container.ReticleAxis);

        m_container.SetMainAction(m_mainAction.Action.IsPressed());
        OnButtonAction(ref m_mainAction);

        m_container.SetSubAction(m_subAction.Action.IsPressed());
        OnButtonAction(ref m_subAction);

        m_container.SetAxialAction(m_axialAction.action.ReadValue<float>());
        m_onAxialAction.Invoke(m_container.AxialAction);

        m_container.SetShoulderLeftAction(m_shoulderLeft.Action.IsPressed());
        OnButtonAction(ref m_shoulderLeft);

        m_container.SetShoulderRightAction(m_shoulderRight.Action.IsPressed());
        OnButtonAction(ref m_shoulderRight);
    }
    private void OnButtonAction(ref ButtonAction buttonAction)
    {
        if (buttonAction.Action.IsPressed())
        {
            buttonAction.time += Time.deltaTime;
            if (buttonAction.time > InputSystem.settings.defaultHoldTime)
            {
                buttonAction.WhileHoldTimeEvent.Invoke(buttonAction.time - InputSystem.settings.defaultHoldTime);
                if (!buttonAction.state)
                {
                    m_logger.Log($"{buttonAction.Action.name} - Hold Started", this);
                    buttonAction.state = true;
                    buttonAction.OnHoldEvent.Invoke(true);
                }
            }
        }
        else if (buttonAction.time > 0)
        {
            if (buttonAction.time <= InputSystem.settings.defaultTapTime)
            {
                m_logger.Log($"{buttonAction.Action.name} - Tap", this);
                buttonAction.OnTapEvent.Invoke();
            }
            else if (buttonAction.time >= InputSystem.settings.defaultHoldTime)
            {
                m_logger.Log($"{buttonAction.Action.name} - Hold Released", this);
                buttonAction.OnHoldEvent.Invoke(false);
                buttonAction.state = false;
            }
            buttonAction.time = 0;
        }
    }
}
