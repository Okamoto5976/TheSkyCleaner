using UnityEngine;

[CreateAssetMenu(fileName = "InputContainer", menuName = "Scriptable Objects/InputContainer")]
public partial class InputContainer : ScriptableObject
{
    [SerializeField] private AxisVector2Container m_movementAxis;
    [SerializeField] private AxisVector2Container m_reticleAxis;
    [SerializeField] private ButtonInput m_mainAction;
    [SerializeField] private ButtonInput m_subAction;
    [SerializeField] private BooleanContainer m_strongAction;
    [SerializeField] private BooleanContainer m_weakAction;
    [SerializeField] private BooleanContainer m_shoulderLeft;
    [SerializeField] private BooleanContainer m_shoulderRight;

    public Vector2 MovementAxis => m_movementAxis.Value;
    public Vector2 ReticleAxis => m_reticleAxis.Value;
    public ButtonInput MainAction => m_mainAction;
    public ButtonInput SubAction => m_subAction;
    public bool StrongAction => m_strongAction.Value;
    public bool WeakAction => m_weakAction.Value;
    public bool ShoulderLeftAction => m_shoulderLeft.Value;
    public bool ShoulderRightAction => m_shoulderRight.Value;


    public void SetMovementAxis(Vector2 vector)
    {
        m_movementAxis.Value = vector;
    }
    public void SetReticleAxis(Vector2 vector)
    {
        m_reticleAxis.Value = vector;
    }
    public void OnMainActionTap()
    {
        m_mainAction.Tap.Trigger();
    }
    public void SetMainActionHoldState(bool state)
    {
        m_mainAction.HoldState.Value = state;
    }
    public void OnSubActionTap()
    {
        m_subAction.Tap.Trigger();
    }
    public void SetSubActionHoldState(bool state)
    {
        m_subAction.HoldState.Value = state;
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