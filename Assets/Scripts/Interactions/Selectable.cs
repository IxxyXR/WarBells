using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Selectable : MonoBehaviour {
	
	private Material _material;
	
	public Color highlightedColor = Color.green;
	public Color touchedColor = Color.yellow;
	
	private Color _originalColor;
	
	private bool _isHighlighted;
	private bool _isTouched;

	void Start () {
		
		_material = GetComponent<Renderer>().material;
		_originalColor = _material.color;
		
		gameObject.AddListener(EventTriggerType.PointerEnter, () => SetIsHighlighted(true));
		gameObject.AddListener(EventTriggerType.PointerExit, () => SetIsHighlighted(false));
		gameObject.AddListener(EventTriggerType.PointerDown, () => SetIsTouched(true));
		gameObject.AddListener(EventTriggerType.PointerUp, () => SetIsTouched(false));
	}
	
	public void SetIsHighlighted(bool value) {
		_isHighlighted = value;
		UpdateColor();
	}

	public void SetIsTouched(bool value) {
		_isTouched = value;
		UpdateColor();
	}
	
	private void UpdateColor() {
		if (_isTouched) {
			_material.color = touchedColor;
		}
		else if (_isHighlighted) {
			_material.color = highlightedColor;
		}
		else {
			_material.color = _originalColor;
		}
	}
}


public static class EventExtensions {

	public static void AddListener(this GameObject gameObject,
		EventTriggerType eventTriggerType,
		UnityAction action) {
		// get the EventTrigger component; if it doesn't exist, create one and add it
		EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>()
		                            ?? gameObject.AddComponent<EventTrigger>();

		// check to see if the entry already exists
		EventTrigger.Entry entry;
		entry = eventTrigger.triggers.Find(e => e.eventID == eventTriggerType);

		if (entry == null) {
			// if it does not, create and add it
			entry = new EventTrigger.Entry {eventID = eventTriggerType};

			// add the entry to the triggers list
			eventTrigger.triggers.Add(entry);
		}

		// add the callback listener
		entry.callback.AddListener(_ => action());
	}

}