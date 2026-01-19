using UnityEngine;

[CreateAssetMenu(fileName = "InputContainer", menuName = "Scriptable Objects/InputContainer")]
public class InputContainer : ScriptableObject
{
    [SerializeField] private AxisVector2Container m_movementAxis;
    [SerializeField] private FloatContainer m_mainAction;

    public Vector2 MovementAxis => m_movementAxis.value;
    public float MainAction => m_mainAction.value;

    public void SetMovementAxis(Vector2 vector)
    {
        m_movementAxis.value = vector;
    }

    public void SetMainAction(float mainAction)
    {
        m_mainAction.value = mainAction;
    }
}
