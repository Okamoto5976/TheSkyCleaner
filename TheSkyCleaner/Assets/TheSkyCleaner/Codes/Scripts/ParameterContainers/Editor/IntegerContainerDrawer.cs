using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(IntegerContainer))]
public class IntegerContainerDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        var objectField = new ObjectField(property.displayName)
        {
            objectType = typeof(IntegerContainer)
        };
        objectField.BindProperty(property);

        var valueLabel = new Label();
        valueLabel.style.paddingLeft = 20;

        container.Add(objectField);
        container.Add(valueLabel);

        objectField.RegisterValueChangedCallback(
            evt =>
            {
                var variable = evt.newValue as IntegerContainer;
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

        var currentVariable = property.objectReferenceValue as IntegerContainer;
        if ( currentVariable != null )
        {
            valueLabel.text = $"Value: {currentVariable.Value}";
        }

        return container;
    }
}
