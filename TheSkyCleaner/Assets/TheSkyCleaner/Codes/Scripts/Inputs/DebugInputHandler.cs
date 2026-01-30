using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DebugInputHandler : MonoBehaviour
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
        [HideInInspector] public bool isHoldSuccessful;
    }

    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Control Events")]
    [SerializeField] private ButtonAction m_debugAction_1;

    private void Awake()
    {
        m_debugAction_1.isHoldSuccessful = false;
    }

    private void Update()
    {
        OnButtonAction(ref m_debugAction_1);
    }
    private void OnButtonAction(ref ButtonAction buttonAction)
    {
        if (buttonAction.Action.IsPressed())
        {
            buttonAction.time += Time.deltaTime;
            if (buttonAction.time > InputSystem.settings.defaultHoldTime)
            {
                buttonAction.WhileHoldTimeEvent.Invoke(buttonAction.time - InputSystem.settings.defaultHoldTime);
                if (!buttonAction.isHoldSuccessful)
                {
                    m_logger.Log($"{buttonAction.Action.name} - Hold Started", this);
                    buttonAction.isHoldSuccessful = true;
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
                buttonAction.isHoldSuccessful = false;
            }
            buttonAction.time = 0;
        }
    }
}
