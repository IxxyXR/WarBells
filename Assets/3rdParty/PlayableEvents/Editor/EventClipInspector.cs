using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace TimelineExtension
{
    [CustomEditor(typeof(EventClip), true)]
    public class EventClipInspector : Editor
    {
        private string m_LastKey;
        private SerializedProperty m_UnityEvent;
        private SerializedProperty m_EventName;

        public void OnEnable()
        {
            m_EventName = serializedObject.FindProperty("eventName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.DelayedTextField(m_EventName);

            var eventProperty = GetEventProperty(m_EventName.stringValue);
            if (!string.IsNullOrEmpty(m_EventName.stringValue) && eventProperty != null)
            {
                eventProperty.serializedObject.Update();
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(eventProperty);
                eventProperty.serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }


        private SerializedProperty GetEventProperty(string key)
        {
            if (TimelineEditor.playableDirector == null)
            {
                m_UnityEvent = null;
                return null;
            }

            if (m_UnityEvent == null || m_LastKey != key)
            {
                var eventTable = TimelineEditor.playableDirector.GetComponent<EventTable>();
                if (eventTable == null)
                    eventTable = TimelineEditor.playableDirector.gameObject.AddComponent<EventTable>();

                SerializedObject o = new SerializedObject(eventTable);
                var evt = eventTable.GetEvent(key, true);
                o.Update();

                var table = o.FindProperty("m_Entries");
                int index = eventTable.IndexOf(evt);
                m_UnityEvent = table.GetArrayElementAtIndex(index).FindPropertyRelative("m_Event");
                m_LastKey = key;
            }
            return m_UnityEvent;
        }
    }
}
