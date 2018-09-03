using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    [CustomPropertyDrawer(typeof(Camera360InputSettings))]
    class Camera360InputPropertyDrawer : InputPropertyDrawer<Camera360InputSettings>
    {
        static ImageSource m_SupportedSources = ImageSource.MainCamera | ImageSource.TaggedCamera;
        string[] m_MaskedSourceNames;

        SerializedProperty m_Source;
        SerializedProperty m_CameraTag;
        SerializedProperty m_FlipFinalOutput;
        SerializedProperty m_StereoSeparation;
        SerializedProperty m_CubeMapSz;
        SerializedProperty m_OutputWidth;
        SerializedProperty m_OutputHeight;
        SerializedProperty m_RenderStereo;

        protected override void Initialize(SerializedProperty property)
        {
            base.Initialize(property);
            
            m_Source = property.FindPropertyRelative("source");
            m_CameraTag = property.FindPropertyRelative("cameraTag");

            m_StereoSeparation = property.FindPropertyRelative("stereoSeparation");
            m_FlipFinalOutput = property.FindPropertyRelative("flipFinalOutput");
            m_CubeMapSz = property.FindPropertyRelative("mapSize");
            m_OutputWidth = property.FindPropertyRelative("outputWidth");
            m_OutputHeight = property.FindPropertyRelative("outputHeight");
            m_RenderStereo = property.FindPropertyRelative("renderStereo");
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(property);
            
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (m_MaskedSourceNames == null)
                    m_MaskedSourceNames = EnumHelper.MaskOutEnumNames<ImageSource>((int)m_SupportedSources);
                
                var index = EnumHelper.GetMaskedIndexFromEnumValue<ImageSource>(m_Source.intValue, (int)m_SupportedSources);
                index = EditorGUILayout.Popup("Source", index, m_MaskedSourceNames);

                if (check.changed)
                    m_Source.intValue = EnumHelper.GetEnumValueFromMaskedIndex<ImageSource>(index, (int)m_SupportedSources);
            }

            if ((ImageSource)m_Source.intValue == ImageSource.TaggedCamera )
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_CameraTag, new GUIContent("Tag"));
                --EditorGUI.indentLevel;
            }
            
            var outputDimensions = new int[2];
            outputDimensions[0] = m_OutputWidth.intValue;
            outputDimensions[1] = m_OutputHeight.intValue;
            
            if (MultiIntField(new GUIContent("360 View Output"), new [] { new GUIContent("W"), new GUIContent("H") }, outputDimensions))
            {
                m_OutputWidth.intValue = outputDimensions[0];
                m_OutputHeight.intValue = outputDimensions[1];
            }
            
            var cubeMapWidth = new int[1];
            cubeMapWidth[0] = m_CubeMapSz.intValue;
            outputDimensions[1] = m_OutputHeight.intValue;
            
            if (MultiIntField(new GUIContent("Cube Map"), new [] { new GUIContent("W") }, cubeMapWidth))
            {
                m_CubeMapSz.intValue = cubeMapWidth[0];
            }
            
            EditorGUILayout.PropertyField(m_RenderStereo, new GUIContent("Stereo"));

            ++EditorGUI.indentLevel;
            using (new EditorGUI.DisabledScope(!m_RenderStereo.boolValue))
            {
                EditorGUILayout.PropertyField(m_StereoSeparation, new GUIContent("Stereo Separation"));
            }
            --EditorGUI.indentLevel;

            EditorGUILayout.PropertyField(m_FlipFinalOutput, new GUIContent("Flip Vertical"));
        }

        static bool MultiIntField(GUIContent label, GUIContent[] subLabels, int[] values)
        {
            var r = EditorGUILayout.GetControlRect();

            var rLabel = r;
            rLabel.width = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(rLabel, label);

            var rContent = r;
            rContent.xMin = rLabel.xMax;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.MultiIntField(rContent, subLabels, values);
            return EditorGUI.EndChangeCheck();
        }
    }
}