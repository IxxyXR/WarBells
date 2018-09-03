using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    [CustomPropertyDrawer(typeof(GameViewInputSettings))]
    class GameViewInputPropertyDrawer : InputPropertyDrawer<GameViewInputSettings>
    {
        SerializedProperty m_RenderSize;
        SerializedProperty m_RenderAspect;

        protected override void Initialize(SerializedProperty property)
        {
            base.Initialize(property);
            
            m_RenderSize = property.FindPropertyRelative("outputHeight");
            m_RenderAspect = property.FindPropertyRelative("outputAspect");
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(property);
            
            m_RenderSize.intValue = ResolutionSelector.Popup("Output Resolution", target.maxSupportedSize, m_RenderSize.intValue);

            if (m_RenderSize.intValue > (int)ImageHeight.Window)
                EditorGUILayout.PropertyField(m_RenderAspect, new GUIContent("Aspect Ratio"));
        }
    }
}