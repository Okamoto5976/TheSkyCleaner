using UnityEngine;

[CreateAssetMenu(fileName = "IntegerThresholdContainer", menuName = "Scriptable Objects/Parameter Containers/IntegerThresholdContainer")]
public class IntegerThresholdContainer : BooleanContainer
{
    enum Operations
    {
        [InspectorName("<")]LessThan,
        [InspectorName("<=")]LessThanEqualTo,
        [InspectorName("==")]EqualTo,
        [InspectorName(">=")]MoreThanEqualTo,
        [InspectorName(">")]MoreThan,
    };

    [SerializeField] private IntegerContainer m_container;
    [SerializeField] private float m_threshold;
    [SerializeField] private Operations m_operator;

    public override bool Value => IsThreshold();

    private bool IsThreshold()
    {
        return m_operator switch
        {
            Operations.LessThan => m_container.Value < m_threshold,
            Operations.LessThanEqualTo => m_container.Value <= m_threshold,
            Operations.EqualTo => m_container.Value == m_threshold,
            Operations.MoreThanEqualTo => m_container.Value >= m_threshold,
            Operations.MoreThan => m_container.Value > m_threshold,
            _ => false,
        };
    }
}
