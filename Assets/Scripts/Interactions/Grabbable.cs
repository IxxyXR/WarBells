using System;
using UnityEngine;

namespace Interactions
{
	public class Grabbable : MonoBehaviour
	{
		private Vector3 originalPosition;
		private Quaternion originalRotation;
		public Transform Target;
		[HideInInspector]
		public bool IsGrabbing;
		[HideInInspector]
		public bool IsGrabbed;

		void Start()
		{
			originalPosition = gameObject.transform.position;
			originalRotation = gameObject.transform.rotation;
		}
	
		public void Grab()
		{
			if (IsGrabbed)
			{
				UnGrab();
			}
			else
			{
				IsGrabbing = true;	
				Tween(Target.transform, FinishGrab);
				foreach (var sibling in GetSiblings())
				{	
					if (sibling.IsGrabbed) {sibling.UnGrab();}
				}			
			}
		}

		public void UnGrab()
		{
			Tween(originalPosition, originalRotation, FinishUnGrab);
		}

		private void Tween(Transform transform, Action callback)
		{
			int id = LeanTween.move(gameObject, transform	, 1f).id;
			LTDescr d = LeanTween.descr(id);
			d?.setOnComplete(callback).setEase(LeanTweenType.easeInOutBack);
		}
		
		private void Tween(Vector3 position, Quaternion rotation, Action callback)
		{
			LeanTween.rotate(gameObject, rotation.eulerAngles, .5f);
			int id = LeanTween.move(gameObject, position, 1f).id;
			LTDescr d = LeanTween.descr(id);
			d?.setOnComplete(callback).setEase(LeanTweenType.easeInOutBack);
		}

		private void FinishGrab()
		{
			IsGrabbing = false;
			IsGrabbed = true;
		}
	
		private void FinishUnGrab()
		{
			IsGrabbing = false;
			IsGrabbed = false;
		}

		private Grabbable[] GetSiblings()
		{
			return gameObject.transform.parent.GetComponentsInChildren<Grabbable>();
		}

	}
}
