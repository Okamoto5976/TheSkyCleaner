using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference m_horizontal;
    [SerializeField] private UnityEvent<float> m_onHorizontalInput;

    [SerializeField] private InputActionReference m_vertical;
    [SerializeField] private UnityEvent<float> m_onVerticalInput;

    [SerializeField] private InputActionReference m_reticle;
    [SerializeField] private UnityEvent<Vector2> m_onReticleInput;

    [SerializeField]private InputActionReference m_mainAction;
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
        m_subActionHoldState = false;
    }

    private void OnEnable()
    {
        // m_mainAction.action.started += OnMainActionInput;
        // m_mainAction.action.performed += OnMainActionInput;
        // m_mainAction.action.canceled += OnMainActionInput;

    }

    private void OnDisable()
    {
        // m_mainAction.action.started -= OnMainActionInput;
        // m_mainAction.action.performed -= OnMainActionInput;
        // m_mainAction.action.canceled -= OnMainActionInput;
    }

    private void Update()
    {
        m_onHorizontalInput.Invoke(m_horizontal.action.ReadValue<float>());
        m_onVerticalInput.Invoke(m_vertical.action.ReadValue<float>());
        m_onAxialAction.Invoke(m_axialAction.action.ReadValue<float>());
        m_onReticleInput.Invoke(m_reticle.action.ReadValue<Vector2>());
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

    // private void OnMainActionInput(InputAction.CallbackContext callbackContext)
    // {
    //     if (callbackContext.duration > InputSystem.settings.defaultHoldTime)
    //     {
    //         if (callbackContext.started)
    //     }
    //     if (callbackContext.canceled)
    //     {
    //         if (callbackContext.duration < InputSystem.settings.defaultTapTime)
    //         {
    //             m_onMainActionInputTap.Invoke();
    //         }
    //         else if (callbackContext.duration > InputSystem.settings.defaultHoldTime)
    //         {
    //             m_onMainActionInputHoldRelease.Invoke();
    //         }
    //     }
    //     if (callbackContext.started && )
    // }
}
