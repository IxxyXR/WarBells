using UnityEngine;
using UnityEngine.Playables;


public class ClickToContinue : MonoBehaviour
{

	public GameObject IconHighlighted;
	public PlayableDirector Timeline;
	public float ResumeAfterSeconds;

	void OnEnable ()
	{
		IconHighlighted.SetActive(false);
		InvokeRepeating(nameof(ToggleIcon), .5f, .5f);
		Timeline.Pause();
		Invoke(nameof(Unpause), ResumeAfterSeconds);
	}

	void Update () {
		if (GvrControllerInput.ClickButtonUp)
		{
			CancelInvoke();
			Unpause();
		}	
	}

	private void ToggleIcon()
	{
		IconHighlighted.SetActive(!IconHighlighted.activeSelf);
	}

	private void Unpause()
	{
		Timeline.Resume();
	}
}
