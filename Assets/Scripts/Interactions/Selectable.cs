using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions
{
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
}
