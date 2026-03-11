using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(BooleanContainer))]
public class BoolContainerDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new();

        ObjectField objectField = new(property.displayName)
        {
            objectType = typeof(BooleanContainer)
        };
        objectField.BindProperty(property);

        Label valueLabel = new();
        valueLabel.style.paddingLeft = 20;

        container.Add(objectField);
        container.Add(valueLabel);

        objectField.RegisterValueChangedCallback(
            evt =>
            {
                var variable = evt.newValue as BooleanContainer;
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

        var currentVariable = property.objectReferenceValue as BooleanContainer;
        if ( currentVariable != null )
        {
            valueLabel.text = $"Value: {currentVariable.Value}";
            currentVariable.OnValueChanged += newValue => valueLabel.text = $"Value: {newValue}";
        }

        return container;
    }
}
