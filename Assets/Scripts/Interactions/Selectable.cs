using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions
{
	public class Selectable : MonoBehaviour {
	
		private Material _material;
	
		public Color highlightedColor = Color.green;
		public AudioClip highlightedSound;
		public Color touchedColor = Color.yellow;
		public AudioClip touchedSound;

		private AudioSource _audioSource;

	
		private Color _originalColor;
	
		private bool _isHighlighted;
		private bool _isTouched;

		void Start () {
		
			_material = GetComponent<Renderer>().material;
			_originalColor = _material.color;
			_audioSource = GetComponent<AudioSource>();
		
			gameObject.AddListener(EventTriggerType.PointerEnter, () => SetIsHighlighted(true));
			gameObject.AddListener(EventTriggerType.PointerExit, () => SetIsHighlighted(false));
			gameObject.AddListener(EventTriggerType.PointerDown, () => SetIsTouched(true));
			gameObject.AddListener(EventTriggerType.PointerUp, () => SetIsTouched(false));
		}
	
		public void SetIsHighlighted(bool value) {
			_isHighlighted = value;
			UpdateColor();
			PlayAudioClipIfExists(highlightedSound);
		}

		public void SetIsTouched(bool value) {
			_isTouched = value;
			UpdateColor();
			PlayAudioClipIfExists(touchedSound);
		}

		private void PlayAudioClipIfExists(AudioClip clip)
		{
			if (clip != null)
			{
				_audioSource.PlayOneShot(clip);				
			}
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
