using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

[CustomPropertyDrawer(typeof(FXBlendBehaviour))]
public class FXBlendDrawer : PropertyDrawer
{
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 1;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty weightProp = property.FindPropertyRelative("weight");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, weightProp);
    }
}
