using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Grabbable : MonoBehaviour {
	
	private float _animationTime = 0;
	private float _animationVelocity;

	// Store this so we can lerp between it and the pushed position
	private Vector3 _originalPosition;

	// Fired once when the button transitions to the down state
	public UnityEvent onPressed;
	// Fired every frame the button is in the down state
	public UnityEvent onDown;
	// Fired once when the button transitions out of the down state
	public UnityEvent onReleased;
	
	void Start () {
		
		_originalPosition = transform.localPosition;

		gameObject.AddListener(EventTriggerType.PointerDown, () => {
			_animationVelocity = 5.0f;
		});
		gameObject.AddListener(EventTriggerType.PointerUp, () => {
			_animationVelocity = -5.0f;
		});	
	}
	
	void Update() {
		
		// Store the previous animation time for later
		float previousAnimationTime = _animationTime;

		UpdateAnimationTime();

		UpdateTransform();

		CheckForEvents(previousAnimationTime);
	}
	
	private void UpdateAnimationTime() {
		
		// If pushed, time moves forwards, if not, time moves backwards
		float timestep = _animationVelocity * Time.deltaTime;
		// Calculate the new animation time and clamp it to [0..1]
		_animationTime = Mathf.Clamp(_animationTime + timestep, 0, 1.0f);
	}

	private void UpdateTransform() {
		
		// We will move the object 0.5 units based on its scale
		float pushDistance = 0.5f * transform.localScale.y;

		// Determine the direction from the object's rotation and the up vector
		Vector3 pushDirection = transform.localRotation * Vector3.up;

		// Determine where the new position will be based on the direction and distance
		Vector3 pushedPosition = _originalPosition - pushDirection * pushDistance;

		// Set the local position to the lerp between the original and pushed position
		transform.localPosition = Vector3.Lerp(_originalPosition, pushedPosition, _animationTime);
	}
	
	private void CheckForEvents(float previousAnimationTime) {
		
		// When the animation time crosses this threshold, it will be considered pushed
		const float pushThreshold = 0.9f;

		// Calculate before and after states
		bool wasDown = previousAnimationTime > pushThreshold;
		bool isDown = _animationTime > pushThreshold;

		// Send press event if first frame pressed
		if (!wasDown && isDown) {
			onPressed.Invoke();
		}

		// Always send down event if down
		if (isDown) {
			onDown.Invoke();
		}

		// Send released event if first frame released
		if (wasDown && !isDown) {
			onReleased.Invoke();
		}
	}

}
