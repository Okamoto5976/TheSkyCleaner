using UnityEngine;

[CreateAssetMenu(fileName = "InputContainer", menuName = "Scriptable Objects/InputContainer")]
public partial class InputContainer : ScriptableObject
{
    [SerializeField] private AxisVector2Container m_movementAxis;
    [SerializeField] private AxisVector2Container m_reticleAxis;
    [SerializeField] private ButtonInput m_mainAction;
    [SerializeField] private ButtonInput m_subAction;
    [SerializeField] private ButtonInput m_strongAction;
    [SerializeField] private ButtonInput m_weakAction;
    [SerializeField] private ButtonInput m_shoulderLeft;
    [SerializeField] private ButtonInput m_shoulderRight;

    public Vector2 MovementAxis => m_movementAxis.Value;
    public Vector2 ReticleAxis => m_reticleAxis.Value;
    public ButtonInput MainAction => m_mainAction;
    public ButtonInput SubAction => m_subAction;
    public ButtonInput StrongAction => m_strongAction;
    public ButtonInput WeakAction => m_weakAction;
    public ButtonInput ShoulderLeftAction => m_shoulderLeft;
    public ButtonInput ShoulderRightAction => m_shoulderRight;


    public void SetMovementAxis(Vector2 vector)
    {
        m_movementAxis.SetValue(vector);
    }
    public void SetReticleAxis(Vector2 vector)
    {
        m_reticleAxis.SetValue(vector);
    }
}