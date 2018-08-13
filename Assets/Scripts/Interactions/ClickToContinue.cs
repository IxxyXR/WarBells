using UnityEngine;
using UnityEngine.Playables;


public class ClickToContinue : MonoBehaviour
{

	public GameObject IconHighlighted;
	public PlayableDirector Timeline;
	public float ResumeAfterSeconds;

	private double _originalTimelineSpeed;

	void OnEnable ()
	{
		_originalTimelineSpeed = Timeline.playableGraph.GetRootPlayable(0).GetSpeed();
		IconHighlighted.SetActive(false);
		InvokeRepeating(nameof(ToggleIcon), .5f, .5f);
		Timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
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
		Timeline.playableGraph.GetRootPlayable(0).SetSpeed(_originalTimelineSpeed);
	}
}
