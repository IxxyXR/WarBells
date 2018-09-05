using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    [CustomPropertyDrawer(typeof(RenderTextureSamplerSettings))]
    class RenderTextureSamplerPropertyDrawer : InputPropertyDrawer<RenderTextureSamplerSettings>
    {
        static ImageSource m_SupportedSources = ImageSource.ActiveCamera | ImageSource.MainCamera | ImageSource.TaggedCamera;
        string[] m_MaskedSourceNames;
        SerializedProperty m_Source;
        SerializedProperty m_RenderSize;
        SerializedProperty m_FinalSize;
        SerializedProperty m_AspectRatio;
        SerializedProperty m_SuperSampling;
        SerializedProperty m_CameraTag;
        SerializedProperty m_FlipFinalOutput;

        protected override void Initialize(SerializedProperty property)
        {
            base.Initialize(property);

            m_Source = property.FindPropertyRelative("source");
            m_RenderSize = property.FindPropertyRelative("renderSize");
            m_AspectRatio = property.FindPropertyRelative("outputAspect");
            m_SuperSampling = property.FindPropertyRelative("superSampling");
            m_FinalSize = property.FindPropertyRelative("outputHeight");
            m_CameraTag = property.FindPropertyRelative("cameraTag");
            m_FlipFinalOutput = property.FindPropertyRelative("flipFinalOutput");
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(property);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (m_MaskedSourceNames == null)
                    m_MaskedSourceNames = EnumHelper.MaskOutEnumNames<ImageSource>((int)m_SupportedSources);
                var index = EnumHelper.GetMaskedIndexFromEnumValue<ImageSource>(m_Source.intValue, (int)m_SupportedSources);
                index = EditorGUILayout.Popup("Object(s) of interest", index, m_MaskedSourceNames);

                if (check.changed)
                    m_Source.intValue = EnumHelper.GetEnumValueFromMaskedIndex<ImageSource>(index, (int)m_SupportedSources);
            }
            
            var inputType = (ImageSource)m_Source.intValue;

            if ((ImageSource)m_Source.intValue == ImageSource.TaggedCamera)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_CameraTag, new GUIContent("Tag"));
                --EditorGUI.indentLevel;
            }

            EditorGUILayout.PropertyField(m_AspectRatio, new GUIContent("Aspect Ratio"));
            EditorGUILayout.PropertyField(m_SuperSampling, new GUIContent("Super sampling"));

            var renderSize = m_RenderSize;

            m_RenderSize.intValue = ResolutionSelector.Popup("Rendering resolution", ImageHeight.x4320p_8K, m_RenderSize.intValue);
                
            if (m_FinalSize.intValue > renderSize.intValue)
                m_FinalSize.intValue = renderSize.intValue;

            m_FinalSize.intValue = ResolutionSelector.Popup("Output Resolution", target.maxSupportedSize, m_FinalSize.intValue);
            
            if (m_FinalSize.intValue == (int)ImageHeight.Window)
                m_FinalSize.intValue = (int)ImageHeight.x720p_HD;
            
            if (m_FinalSize.intValue > renderSize.intValue)
                renderSize.intValue = m_FinalSize.intValue;
            
            EditorGUILayout.PropertyField(m_FlipFinalOutput, new GUIContent("Flip Vertical"));
            
            if (Options.verboseMode)
                EditorGUILayout.LabelField("Color Space", target.colorSpace.ToString());
        }
    }

}
