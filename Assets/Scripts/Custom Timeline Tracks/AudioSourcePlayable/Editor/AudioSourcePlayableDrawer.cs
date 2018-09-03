using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[CustomPropertyDrawer(typeof(AudioSourcePlayableBehaviour))]
public class AudioSourcePlayableDrawer : PropertyDrawer
{
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 3;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty volumeProp = property.FindPropertyRelative("volume");
        SerializedProperty enabledProp = property.FindPropertyRelative("enabled");
        SerializedProperty timeProp = property.FindPropertyRelative("startTime");
        SerializedProperty clipProp = property.FindPropertyRelative("clip");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, volumeProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, enabledProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, clipProp);
        
        
    }
}
