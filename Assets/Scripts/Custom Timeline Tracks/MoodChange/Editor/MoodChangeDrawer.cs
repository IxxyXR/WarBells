using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MoodChangeBehaviour))]
public class MoodChangeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 5;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty LightColorProp = property.FindPropertyRelative("LightColor");
        SerializedProperty SkyboxTintProp = property.FindPropertyRelative("SkyboxTint");
        SerializedProperty GradientFogProp = property.FindPropertyRelative("GradientFog");
        SerializedProperty DistanceFogColorProp = property.FindPropertyRelative("DistanceFogColor");
        SerializedProperty DistanceFogStartProp = property.FindPropertyRelative("DistanceFogStart");
        SerializedProperty DistanceFogEndProp = property.FindPropertyRelative("DistanceFogEnd");
        SerializedProperty FogDensityProp = property.FindPropertyRelative("FogDensity");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.PropertyField(singleFieldRect, LightColorProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, SkyboxTintProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight * 2;

        EditorGUI.PropertyField(singleFieldRect, DistanceFogColorProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, DistanceFogStartProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, DistanceFogEndProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight * 2;

        EditorGUI.PropertyField(singleFieldRect, GradientFogProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, FogDensityProp);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
    }
}
