using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputContainer m_container;

    [SerializeField] private InputActionReference m_horizontal;
    [SerializeField] private UnityEvent<float> m_onHorizontalInput;

    [SerializeField] private InputActionReference m_vertical;
    [SerializeField] private UnityEvent<float> m_onVerticalInput;

    [SerializeField] private InputActionReference m_reticle;
    [SerializeField] private UnityEvent<Vector2> m_onReticleInput;

    [SerializeField] private InputActionReference m_mainAction;
    [SerializeField] private UnityEvent m_onMainActionInputTap;
    [SerializeField] private UnityEvent<float> m_onMainActionInputHold;
    [SerializeField] private UnityEvent<bool> m_onMainActionInputHoldState;

    [SerializeField] private InputActionReference m_subAction;
    [SerializeField] private UnityEvent m_onSubActionInputTap;
    [SerializeField] private UnityEvent<float> m_onSubActionInputHold;
    [SerializeField] private UnityEvent<bool> m_onSubActionInputHoldState;

    [SerializeField] private InputActionReference m_axialAction;
    [SerializeField] private UnityEvent<float> m_onAxialAction;

    private float m_mainActionTime;
    private bool m_mainActionHoldState;

    private float m_subActionTime;
    private bool m_subActionHoldState;

    private void Awake()
    {
        m_mainActionHoldState = false;
    }

    private void Update()
    {
        m_container.SetMovementAxis(new Vector2(m_horizontal.action.ReadValue<float>(), m_vertical.action.ReadValue<float>()));


        m_onHorizontalInput.Invoke(m_horizontal.action.ReadValue<float>());
        m_onVerticalInput.Invoke(m_vertical.action.ReadValue<float>());
        m_onAxialAction.Invoke(m_axialAction.action.ReadValue<float>());
        OnButtonAction(m_mainAction, m_onMainActionInputTap, m_onMainActionInputHold, m_onSubActionInputHoldState, ref m_mainActionTime, ref m_mainActionHoldState);
        OnButtonAction(m_subAction, m_onSubActionInputTap, m_onSubActionInputHold, m_onSubActionInputHoldState, ref m_subActionTime, ref m_subActionHoldState);
    }
    private void OnButtonAction(InputActionReference inputActionReference, UnityEvent tapEvent, UnityEvent<float> holdTimeEvent, UnityEvent<bool> holdStateEvent, ref float time, ref bool holdState)
    {
        if (inputActionReference.action.IsPressed())
        {
            time += Time.deltaTime;
            if (time > InputSystem.settings.defaultHoldTime)
            {
                holdTimeEvent.Invoke(time - InputSystem.settings.defaultHoldTime);
                if (!holdState)
                {
                    Debug.Log("Hold Started");
                    holdState = true;
                    holdStateEvent.Invoke(true);
                }
            }
        }
        else if (time > 0)
        {
            if (time <= InputSystem.settings.defaultTapTime)
            {
                Debug.Log("Tap");
                tapEvent.Invoke();
            }
            else if (time >= InputSystem.settings.defaultHoldTime)
            {
                Debug.Log("Hold Released");
                holdStateEvent.Invoke(false);
                holdState = false;
            }
            time = 0;
        }
    }
}
