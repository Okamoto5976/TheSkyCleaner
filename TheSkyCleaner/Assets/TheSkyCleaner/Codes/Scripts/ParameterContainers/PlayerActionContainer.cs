using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerActionContainer", menuName = "Scriptable Objects/PlayerActionContainer")]
public partial class PlayerActionContainer : ScriptableObject
{

    [SerializeField] private AxisVector3Container m_movementAxis;
    [SerializeField] private AxisVector2Container m_reticleAxis;
    [SerializeField] private HybridAction m_mainAction;
    [SerializeField] private BooleanContainer m_subAction;
    [SerializeField] private BooleanContainer m_strongAction;
    [SerializeField] private BooleanContainer m_weakAction;
    [SerializeField] private BooleanContainer m_shoulderLeft;
    [SerializeField] private BooleanContainer m_shoulderRight;

    public AxisVector3Container MovementAxis => m_movementAxis;
    public AxisVector2Container ReticleAxis => m_reticleAxis;
    public HybridAction MainAction => m_mainAction;
    public BooleanContainer SubAction => m_subAction;
    public BooleanContainer StrongAction => m_strongAction;
    public BooleanContainer WeakAction => m_weakAction;
    public BooleanContainer ShoulderLeftAction => m_shoulderLeft;
    public BooleanContainer ShoulderRightAction => m_shoulderRight;


    public void SetMovementAxis(Vector3 vector)
    {
        m_movementAxis.Value = vector;
    }
    public void SetReticleAxis(Vector2 vector)
    {
        m_reticleAxis.Value = vector;
    }
    public void OnMainActionTap()
    {
        if (m_mainAction.tap != null)
        {
            m_mainAction.tap.Trigger();
        }
    }
    public void SetMainActionHoldState(bool state)
    {
        if (m_mainAction.holdState != null)
        {
            m_mainAction.holdState.Value = state;
        }
    }
    public void SetSubAction(bool subAction)
    {
        m_subAction.Value = subAction;
    }
    public void SetStrongAction(bool strongAction)
    {
        m_strongAction.Value = strongAction;
    }
    public void SetWeakAction(bool weakAction)
    {
        m_weakAction.Value = weakAction;
    }
    public void SetShoulderLeftAction(bool shoulderLeft)
    {
        m_shoulderLeft.Value = shoulderLeft;
    }
    public void SetShoulderRightAction(bool shoulderRight)
    {
        m_shoulderRight.Value = shoulderRight;
    }
}