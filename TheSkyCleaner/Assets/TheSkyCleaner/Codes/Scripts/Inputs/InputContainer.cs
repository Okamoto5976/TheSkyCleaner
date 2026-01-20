using UnityEngine;

[CreateAssetMenu(fileName = "InputContainer", menuName = "Scriptable Objects/InputContainer")]
public class InputContainer : ScriptableObject
{
    [SerializeField] private AxisVector2Container m_movementAxis;
    [SerializeField] private AxisVector2Container m_reticleAxis;
    [SerializeField] private BooleanContainer m_mainAction;
    [SerializeField] private BooleanContainer m_subAction;
    [SerializeField] private BooleanContainer m_strongAction;
    [SerializeField] private BooleanContainer m_weakAction;
    [SerializeField] private BooleanContainer m_shoulderLeft;
    [SerializeField] private BooleanContainer m_shoulderRight;

    public Vector2 MovementAxis => m_movementAxis.value;
    public Vector2 ReticleAxis => m_reticleAxis.value;
    public bool MainAction => m_mainAction.value;
    public bool SubAction => m_subAction.value;
    public bool StrongAction => m_strongAction.value;
    public bool WeakAction => m_weakAction.value;
    public bool ShoulderLeftAction => m_shoulderLeft.value;
    public bool ShoulderRightAction => m_shoulderRight.value;


    public void SetMovementAxis(Vector2 vector)
    {
        m_movementAxis.value = vector;
    }
    public void SetReticleAxis(Vector2 vector)
    {
        m_reticleAxis.value = vector;
    }
    public void SetMainAction(bool mainAction)
    {
        m_mainAction.value = mainAction;
    }
    public void SetSubAction(bool subAction)
    {
        m_subAction.value = subAction;
    }
    public void SetStrongAction(bool strongAction)
    {
        m_strongAction.value = strongAction;
    }
    public void SetWeakAction(bool weakAction)
    {
        m_weakAction.value = weakAction;
    }
    public void SetShoulderLeftAction(bool shoulderLeft)
    {
        m_shoulderLeft.value = shoulderLeft;
    }
    public void SetShoulderRightAction(bool shoulderRight)
    {
        m_shoulderRight.value = shoulderRight;
    }
}