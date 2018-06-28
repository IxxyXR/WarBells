using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UISelectable : MonoBehaviour
{

	private Button _button;
	
	private bool _isHighlighted;
	private bool _isTouched;

	void Start () {
		
		_button = GetComponent<Button>();
		
		gameObject.AddListener(EventTriggerType.PointerEnter, () => SetIsHighlighted(true));
		gameObject.AddListener(EventTriggerType.PointerExit, () => SetIsHighlighted(false));
		gameObject.AddListener(EventTriggerType.PointerDown, () => SetIsTouched(true));
		gameObject.AddListener(EventTriggerType.PointerUp, () => SetIsTouched(false));
	}
	
	public void SetIsHighlighted(bool value) {
		_isHighlighted = value;
	}

	public void SetIsTouched(bool value) {
		_isTouched = value;
	}
	

}


