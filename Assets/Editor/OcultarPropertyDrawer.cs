// Original: Brecht Lecluyse (www.brechtos.com)
// YerkoAndrei
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(OcultarAttribute))]
public class OcultarPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        OcultarAttribute condHAtt = (OcultarAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;

        if (!condHAtt.deshabilitar || enabled)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        OcultarAttribute condHAtt = (OcultarAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (!condHAtt.deshabilitar || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(OcultarAttribute ocultarAtt, SerializedProperty property)
    {
        if (ocultarAtt.tipoOcultar == OcultarAttribute.TipoOcultar.ocultar)
            return false;

        bool enabled = true;
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, ocultarAtt.variableCondicional); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            switch (ocultarAtt.tipoOcultar)
            {
                case OcultarAttribute.TipoOcultar.desaparecerPorBool:
                    enabled = sourcePropertyValue.boolValue;
                    break;
                case OcultarAttribute.TipoOcultar.desaparecerPorInt:
                    enabled = (sourcePropertyValue.intValue == ocultarAtt.valorInt);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Editor: no hay variable: " + ocultarAtt.variableCondicional);
        }

        return enabled;
    }
}
