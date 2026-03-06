using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    [System.Serializable]
    struct ButtonAction
    {
        [SerializeField] private InputActionReference inputActionReference; 
        [SerializeField] private ButtonInput variableContainer;

        public readonly InputAction Action => inputActionReference.action;
        public readonly ButtonInput Container => variableContainer;

        [HideInInspector] public float time;
        [HideInInspector] public bool isHoldSuccessful;
        [HideInInspector] public bool isPressed;
    }

    [Header("Logger")]
    [SerializeField] private Logger m_logger;
    [Header("Variable Container")]
    [SerializeField] private InputContainer m_container;

    [Header("Control Events")]
    [SerializeField] private InputActionReference m_movementAxisAction;
    [SerializeField] private InputActionReference m_reticleAxis;

    [SerializeField] private ButtonAction m_mainAction;
    [SerializeField] private ButtonAction m_subAction;
    [SerializeField] private ButtonAction m_strongAction;
    [SerializeField] private ButtonAction m_weakAction;
    [SerializeField] private ButtonAction m_shoulderLeft;
    [SerializeField] private ButtonAction m_shoulderRight;
    [SerializeField] private ButtonAction m_bombAction;

    private Vector2 m_movementAxis;

    private void Awake()
    {
        m_mainAction.isHoldSuccessful = false;
        m_subAction.isHoldSuccessful = false;
        m_strongAction.isHoldSuccessful = false;
        m_weakAction.isHoldSuccessful = false;
        m_shoulderLeft.isHoldSuccessful = false;
        m_shoulderRight.isHoldSuccessful = false;
        m_bombAction.isHoldSuccessful = false;
    }

    private void Update()
    {
        m_container.SetMovementAxis(m_movementAxisAction.action.ReadValue<Vector2>());
        m_container.SetReticleAxis(m_reticleAxis.action.ReadValue<Vector2>());
        OnButtonAction(ref m_mainAction);
        OnButtonAction(ref m_subAction);
        OnButtonAction(ref m_strongAction);
        OnButtonAction(ref m_weakAction);
        OnButtonAction(ref m_shoulderLeft);
        OnButtonAction(ref m_shoulderRight);
        OnButtonAction(ref m_bombAction);
    }
    private void OnButtonAction(ref ButtonAction buttonAction)
    {
        if (IsActionNotFound(buttonAction)) return;
        if (buttonAction.Action.IsPressed())
        {
            if (!buttonAction.isPressed)
            {
                buttonAction.isPressed = true;
                if (!IsContainerNotFound(buttonAction.Container.OnPress, "OnPress"))
                {
                    buttonAction.Container.OnPress.Trigger();
                }
            }

            buttonAction.time += Time.deltaTime;

            if (buttonAction.isHoldSuccessful) return;

            if (buttonAction.time > InputSystem.settings.defaultHoldTime)
            {
                buttonAction.isHoldSuccessful = true;
                m_logger.Log($"{buttonAction.Action.name} - Hold Started", this);

                if (!IsContainerNotFound(buttonAction.Container.HoldState, "HoldState"))
                {
                    buttonAction.Container.HoldState.SetValue(true);
                }
            }

            return;
        }
        
        if (buttonAction.time > 0)
        {
            if (buttonAction.time <= InputSystem.settings.defaultTapTime)
            {
                m_logger.Log($"{buttonAction.Action.name} - Tap", this);

                if (!IsContainerNotFound(buttonAction.Container.Tap, "Tap"))
                {
                    buttonAction.Container.Tap.Trigger();
                }
            }
            else if (buttonAction.time >= InputSystem.settings.defaultHoldTime)
            {
                m_logger.Log($"{buttonAction.Action.name} - Hold Released", this);
                buttonAction.isHoldSuccessful = false;

                if (!IsContainerNotFound(buttonAction.Container.HoldState, "HoldState"))
                {
                    buttonAction.Container.HoldState.SetValue(false);
                }
            }
            buttonAction.time = 0;
            buttonAction.isPressed = false;
        }
    }

    private bool IsActionNotFound(ButtonAction buttonAction)
    {
        if (buttonAction.Action != null) return false;

        m_logger.LogWarning($"Input Action not found", this);
        return true;
    }

    private bool IsContainerNotFound(ScriptableObject rso, string containerType)
    {
        if (rso != null) return false;

        m_logger.LogWarning($"Container not found : missing {containerType}", this);
        return true;
    }
}
