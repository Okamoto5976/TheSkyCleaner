using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(FloatContainer))]
public class FloatContainerDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        var objectField = new ObjectField(property.displayName)
        {
            objectType = typeof(FloatContainer)
        };
        objectField.BindProperty(property);

        var valueLabel = new Label();
        valueLabel.style.paddingLeft = 20;

        container.Add(objectField);
        container.Add(valueLabel);

        objectField.RegisterValueChangedCallback(
            evt =>
            {
                var variable = evt.newValue as FloatContainer;
                if (variable != null)
                {
                    valueLabel.text = $"Value : {variable.Value}";
                }
                else
                {
                    valueLabel.text = string.Empty;
                }
            }
        );

        var currentVariable = property.objectReferenceValue as FloatContainer;
        if ( currentVariable != null )
        {
            valueLabel.text = $"Value: {currentVariable.Value}";
        }

        return container;
    }
}
