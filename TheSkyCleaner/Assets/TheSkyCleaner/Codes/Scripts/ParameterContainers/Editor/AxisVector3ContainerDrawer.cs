using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(AxisVector3Container))]
public class AxisVector3ContainerDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        var objectField = new ObjectField(property.displayName)
        {
            objectType = typeof(AxisVector3Container)
        };
        objectField.BindProperty(property);

        var valueLabel = new Label();
        valueLabel.style.paddingLeft = 20;

        container.Add(objectField);
        container.Add(valueLabel);

        objectField.RegisterValueChangedCallback(
            evt =>
            {
                var variable = evt.newValue as AxisVector3Container;
                if (variable != null)
                {
                    valueLabel.text = $"Value : {variable.Value}";
                    variable.OnValueChanged += newValue => valueLabel.text = $"Value : {newValue}";
                }
                else
                {
                    valueLabel.text = string.Empty;
                }
            }
        );

        var currentVariable = property.objectReferenceValue as AxisVector3Container;
        if ( currentVariable != null )
        {
            valueLabel.text = $"Value: {currentVariable.Value}";
            currentVariable.OnValueChanged += newValue => valueLabel.text = $"Value: {newValue}";
        }

        return container;
    }
}
