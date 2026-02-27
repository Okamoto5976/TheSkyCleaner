using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MassBooleanContainer", menuName = "Scriptable Objects/Parameter Containers/MassBooleanContainer")]
public class MassBooleanContainer : BooleanContainer
{
    [SerializeField] private List<BooleanContainer> m_booleanContainers;
    private enum Method
    {
        [InspectorName("At least one is true")]or,
        [InspectorName("When all is true")]and,
    }
    [SerializeField] private Method m_method;


    public override bool Value => CollapseValue();

    protected bool CollapseValue()
    {
        switch (m_method)
        {
            case Method.and:
                return !m_booleanContainers
            .Exists(x => x.Value == false);
            case Method.or:
                return m_booleanContainers
            .Exists(x => x.Value == true);
        }
        return false;
    }
}
