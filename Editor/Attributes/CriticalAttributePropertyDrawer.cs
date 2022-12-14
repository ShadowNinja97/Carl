using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CriticalAttribute))]
public class CriticalAttributePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CriticalAttribute ca = attribute as CriticalAttribute;
        if (property.objectReferenceValue == null)
        {
            GUI.color = ca.color;
            EditorGUI.PropertyField(position, property, label);
            GUI.color = Color.white;

            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                Debug.LogError($"{nameof(CriticalAttribute)} cannot be null! {property.propertyPath}");
            }

        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }

    }


}

