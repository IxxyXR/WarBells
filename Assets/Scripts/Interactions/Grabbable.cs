using System;
using UnityEngine;


public class Grabbable : MonoBehaviour
{
	private Vector3 originalPosition;
	public Transform Target;
	public Vector3 Offset;
	[HideInInspector]
	public bool IsGrabbing;
	[HideInInspector]
	public bool IsGrabbed;

	void Start()
	{
		originalPosition = gameObject.transform.position;
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
			Tween(Target.position + Offset, FinishGrab);
			foreach (var sibling in GetSiblings())
			{	
				if (sibling.IsGrabbed) {sibling.UnGrab();}
			}			
		}
	}

	public void UnGrab()
	{
		Tween(originalPosition, FinishUnGrab);
	}

	private void Tween(Vector3 position, Action callback)
	{
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
