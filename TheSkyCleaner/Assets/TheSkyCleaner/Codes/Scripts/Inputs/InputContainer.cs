using UnityEngine;

[CreateAssetMenu(fileName = "InputContainer", menuName = "Scriptable Objects/InputContainer")]
public class InputContainer : ScriptableObject
{
    [SerializeField] private AxisVector2Container m_movementAxis;
    [SerializeField] private AxisVector2Container m_reticleAxis;
    [SerializeField] private BooleanContainer m_mainAction;
    [SerializeField] private BooleanContainer m_subAction;
    [SerializeField] private BooleanContainer m_shoulderLeft;
    [SerializeField] private BooleanContainer m_shoulderRight;
    [SerializeField] private FloatContainer m_axialAction;

    public Vector2 MovementAxis => m_movementAxis.value;
    public Vector2 ReticleAxis => m_reticleAxis.value;
    public bool MainAction => m_mainAction.value;
    public bool SubAction => m_subAction.value;
    public bool ShoulderLeftAction => m_shoulderLeft.value;
    public bool ShoulderRightAction => m_shoulderRight.value;
    public float AxialAction => m_axialAction.value;


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
    public void SetShoulderLeftAction(bool shoulderLeft)
    {
        m_shoulderLeft.value = shoulderLeft;
    }
    public void SetShoulderRightAction(bool shoulderRight)
    {
        m_shoulderRight.value = shoulderRight;
    }
    public void SetAxialAction(float axialAction)
    {
        m_axialAction.value = axialAction;
    }
}