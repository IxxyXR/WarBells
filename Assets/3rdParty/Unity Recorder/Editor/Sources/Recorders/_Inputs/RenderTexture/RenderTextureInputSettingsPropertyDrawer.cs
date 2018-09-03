using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    [CustomPropertyDrawer(typeof(RenderTextureInputSettings))]
    class RenderTextureInputSettingsPropertyDrawer : InputPropertyDrawer<RenderTextureInputSettings>
    {
        SerializedProperty m_SourceRTxtr;

        protected override void Initialize(SerializedProperty property)
        {
            base.Initialize(property);

            if (m_SourceRTxtr == null)
                m_SourceRTxtr = property.FindPropertyRelative("renderTexture");
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(property);
            
            EditorGUILayout.PropertyField(m_SourceRTxtr, new GUIContent("Render Texture"));

            var res = "N/A";
            if (m_SourceRTxtr.objectReferenceValue != null)
            {
                var renderTexture = (RenderTexture)m_SourceRTxtr.objectReferenceValue;
                res = string.Format("{0}x{1}", renderTexture.width, renderTexture.height);
            }
            EditorGUILayout.LabelField("Resolution", res);
        }
    }
}