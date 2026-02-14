using UnityEngine;

[CreateAssetMenu(fileName = "TransformingFloatContainer", menuName = "Scriptable Objects/Parameter Containers/TransformingFloatContainer")]
public class TransformingFloatContainer : FloatContainer
{
    [SerializeField] protected FloatContainer m_floatContainer;
    [SerializeField] protected Vector3 m_transformingValues;

    private float Transform(float val)
    {
        float newVal;
        if (val > 0)
        {
            newVal = (float)(val * (m_transformingValues.z - m_transformingValues.y) + m_transformingValues.y);
        }
        else if (val < 0)
        {
            newVal = (float)(val * (m_transformingValues.y - m_transformingValues.x) + m_transformingValues.y);
        }
        else
        {
            newVal = m_transformingValues.y;
        }
        m_value = newVal;
        return newVal;
    }
    public override float Value => Transform(m_floatContainer.Value);
}
