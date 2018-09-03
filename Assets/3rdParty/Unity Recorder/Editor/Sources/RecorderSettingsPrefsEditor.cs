using UnityEngine;

namespace UnityEditor.Recorder
{
    [CustomEditor(typeof(RecorderControllerSettings))]
    class RecorderSettingsPrefsEditor : Editor
    {
        SerializedProperty m_RecordModeProperty;
        
        SerializedProperty m_PlaybackProperty;
        SerializedProperty m_FrameRateTypeProperty;
        SerializedProperty m_CustomFrameRateValueProperty;
        
        SerializedProperty m_StartFrameProperty;
        SerializedProperty m_EndFrameProperty;
        SerializedProperty m_StartTimeProperty;
        SerializedProperty m_EndTimeProperty;
        
        SerializedProperty m_CapFrameRateProperty;

        GenericMenu m_FrameRateMenu;

        static class Styles
        {
            internal static readonly GUIContent RecordModeLabel  = new GUIContent("Record Mode");
            internal static readonly GUIContent SingleFrameLabel = new GUIContent("Frame number");
            internal static readonly GUIContent FirstFrameLabel  = new GUIContent("First frame");
            internal static readonly GUIContent LastFrameLabel   = new GUIContent("Last frame");
            internal static readonly GUIContent StartTimeLabel   = new GUIContent("Start (sec)");
            internal static readonly GUIContent EndTimeLabel     = new GUIContent("End (sec)");
            
            internal static readonly GUIContent FrameRateTitle   = new GUIContent("Frame Rate");
            internal static readonly GUIContent PlaybackLabel    = new GUIContent("Playback");
            internal static readonly GUIContent TargetFPSLabel   = new GUIContent("Target");
            internal static readonly GUIContent MaxFPSLabel      = new GUIContent("Max Frame Rate");
            internal static readonly GUIContent CapFPSLabel      = new GUIContent("Cap Frame Rate");
            internal static readonly GUIContent ValueLabel       = new GUIContent("Value");
        }

        void OnEnable()
        {
            if (target == null)
                return;
            
            m_RecordModeProperty = serializedObject.FindProperty("m_RecordMode");
            m_PlaybackProperty = serializedObject.FindProperty("m_FrameRatePlayback");
            m_FrameRateTypeProperty  = serializedObject.FindProperty("m_FrameRateType");
            m_CustomFrameRateValueProperty = serializedObject.FindProperty("m_CustomFrameRateValue");
            m_StartFrameProperty = serializedObject.FindProperty("m_StartFrame");
            m_EndFrameProperty = serializedObject.FindProperty("m_EndFrame");
            m_StartTimeProperty = serializedObject.FindProperty("m_StartTime");
            m_EndTimeProperty = serializedObject.FindProperty("m_EndTime");
            m_CapFrameRateProperty = serializedObject.FindProperty("m_CapFrameRate");
        }

        public override void OnInspectorGUI()
        {
            RecordModeGUI();
            EditorGUILayout.Separator();
            FrameRateGUI();
        }

        internal bool RecordModeGUI()
        {           
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_RecordModeProperty, Styles.RecordModeLabel);

            ++EditorGUI.indentLevel;
            
            switch ((RecordMode)m_RecordModeProperty.enumValueIndex)
            {
                case RecordMode.Manual:
                {
                    // Nothing
                    break;
                }
                    
                case RecordMode.SingleFrame:
                {
                    PositiveIntPropery(m_StartFrameProperty, Styles.SingleFrameLabel, 0);
                    m_EndFrameProperty.intValue = m_StartFrameProperty.intValue;
                    break;
                }
                    
                case RecordMode.FrameInterval:
                {
                    PositiveIntPropery(m_StartFrameProperty, Styles.FirstFrameLabel, 0);
                    PositiveIntPropery(m_EndFrameProperty, Styles.LastFrameLabel, m_StartFrameProperty.intValue);
                    break;
                }
                    
                case RecordMode.TimeInterval:
                {
                    PositiveFloatPropery(m_StartTimeProperty, Styles.StartTimeLabel, 0.0f);
                    PositiveFloatPropery(m_EndTimeProperty, Styles.EndTimeLabel, m_StartTimeProperty.floatValue);
                    break;
                } 
            }
            
            --EditorGUI.indentLevel;            
            
            serializedObject.ApplyModifiedProperties();
            
            return GUI.changed;
        }
        
        internal bool FrameRateGUI()
        {           
            serializedObject.Update();
            
            EditorGUILayout.LabelField(Styles.FrameRateTitle);
            
            ++EditorGUI.indentLevel;
            
            EditorGUILayout.PropertyField(m_PlaybackProperty, Styles.PlaybackLabel);

            var variableFPS = m_PlaybackProperty.enumValueIndex == (int) FrameRatePlayback.Variable;
            
            EditorGUILayout.PropertyField(m_FrameRateTypeProperty, variableFPS ? Styles.MaxFPSLabel : Styles.TargetFPSLabel);

            if (m_FrameRateTypeProperty.enumValueIndex == (int) FrameRateType.FR_CUSTOM)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_CustomFrameRateValueProperty, Styles.ValueLabel);
                --EditorGUI.indentLevel;
            }
            
            if (!variableFPS)
            {
                EditorGUILayout.PropertyField(m_CapFrameRateProperty, Styles.CapFPSLabel);       
            }
            
            --EditorGUI.indentLevel;
            
            serializedObject.ApplyModifiedProperties();

            return GUI.changed;
        }

        static void PositiveIntPropery(SerializedProperty property, GUIContent label, int minValue)
        {
            property.intValue = System.Math.Max(EditorGUILayout.IntField(label, property.intValue), minValue);
        }
        
        static void PositiveFloatPropery(SerializedProperty property, GUIContent label, float minValue)
        {
            property.floatValue = Mathf.Max(EditorGUILayout.FloatField(label, property.floatValue), minValue);
        }
    }
}