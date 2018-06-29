using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Grabbable : MonoBehaviour {
	
	private float _animationTime = 0;
	private float _animationVelocity;

	private Vector3 _originalPosition;

	public UnityEvent onPressed;
	public UnityEvent onDown;
	public UnityEvent onReleased;
	
	void Start () {
		_originalPosition = transform.localPosition;
		gameObject.AddListener(EventTriggerType.PointerDown, onPointerDown);
		gameObject.AddListener(EventTriggerType.PointerUp, onPointerUp);	
	}

	void onPointerDown()
	{
		_animationVelocity = 5.0f;
	}

	void onPointerUp()
	{
		_animationVelocity = -5.0f;
	}
	
	void Update() {		
		float previousAnimationTime = _animationTime;
		UpdateAnimationTime();
		UpdateTransform();
		CheckForEvents(previousAnimationTime);
	}
	
	private void UpdateAnimationTime() {		
		float timestep = _animationVelocity * Time.deltaTime;
		_animationTime = Mathf.Clamp(_animationTime + timestep, 0, 1.0f);
	}

	private void UpdateTransform() {
		float pushDistance = 1f * transform.localScale.y;
		Vector3 pushDirection = transform.localRotation * Vector3.forward;
		Vector3 pushedPosition = _originalPosition - pushDirection * pushDistance;
		transform.localPosition = Vector3.Lerp(_originalPosition, pushedPosition, _animationTime);
	}
	
	private void CheckForEvents(float previousAnimationTime) {
		const float pushThreshold = 0.9f;
		bool wasDown = previousAnimationTime > pushThreshold;
		bool isDown = _animationTime > pushThreshold;
		if (!wasDown && isDown) {onPressed.Invoke();}
		if (isDown) {onDown.Invoke();}
		if (wasDown && !isDown) {onReleased.Invoke();}
	}

}
