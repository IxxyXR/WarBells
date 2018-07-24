using UnityEditor;
using UnityEngine;

namespace Interactions.Editor
{
	[CustomEditor(typeof(ResumeAfterTime))]
	public class ResumeAfterTimeEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
        
			ResumeAfterTime script = (ResumeAfterTime)target;
			if(GUILayout.Button("Resume"))
			{
				script.Resume();
			}
		}
	}
}