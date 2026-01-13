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
    [SerializeField]private InputActionReference m_mainAction;
    [SerializeField] private UnityEvent m_onMainActionInputTap;
    [SerializeField] private UnityEvent<float> m_onMainActionInputHold;
    [SerializeField] private UnityEvent<bool> m_onMainActionInputHoldState;

    [SerializeField] private InputActionReference m_axialAction;
    [SerializeField] private UnityEvent<float> m_onAxialAction;

    private float m_mainActionTime;
    private bool m_mainActionHoldState;

    private void Awake()
    {
        m_mainActionHoldState = false;
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
        if (m_mainAction.action.IsPressed())
        {
            m_mainActionTime += Time.deltaTime;
            if (m_mainActionTime > InputSystem.settings.defaultHoldTime)
            {
                m_onMainActionInputHold.Invoke(m_mainActionTime - InputSystem.settings.defaultHoldTime);
                if (!m_mainActionHoldState)
                {
                    Debug.Log("Hold Started");
                    m_mainActionHoldState = true;
                    m_onMainActionInputHoldState.Invoke(true);
                }
            }
        }
        else if (m_mainActionTime > 0)
        {
            if (m_mainActionTime <= InputSystem.settings.defaultTapTime)
            {
                Debug.Log("Tap");
                m_onMainActionInputTap.Invoke();
            }
            else if (m_mainActionTime >= InputSystem.settings.defaultHoldTime)
            {
                Debug.Log("Hold Released");
                m_onMainActionInputHoldState.Invoke(false);
                m_mainActionHoldState = false;
            }
            m_mainActionTime = 0;
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
