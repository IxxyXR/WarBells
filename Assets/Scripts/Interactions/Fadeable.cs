using UnityEngine;
using UnityEngine.Playables;

public class Fadeable : MonoBehaviour
{

	public float Speed = 3;
	private bool isVisible = true;
	public AudioClip AppearSound;
	public AudioClip VoiceOver;
	private PlayableDirector _mainTimeline;
	private bool voiceoverFinished;

	void Start ()
	{
		DoFade(0, 0);
		isVisible = false;
		_mainTimeline = GameObject.FindGameObjectWithTag("MainTimeline").GetComponent<PlayableDirector>();
	}

	void Update()
	{
		if (GvrControllerInput.ClickButtonUp && voiceoverFinished)
		{
			FadeOut();
		}	
	}

	private void OnEnable()
	{
		if (!isVisible)
		{
			FadeIn();
		}
	}

	public void DoFade(float alpha, float speed)
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			var g = gameObject.transform.GetChild(i).gameObject;
			LeanTween.alpha(g, alpha, speed);
		}
	}
	
	public void FadeIn()
	{
		DoFade(1, Speed);
		gameObject.GetComponent<AudioSource>().PlayOneShot(AppearSound);
		Invoke(nameof(PlayVoiceOver), 3f);
		_mainTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
		isVisible = true;
	}
	
	public void PlayVoiceOver()
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot(VoiceOver);		
		Invoke(nameof(VoiceoverFinished), VoiceOver.length);
	}

	public void VoiceoverFinished()
	{
		voiceoverFinished = true;
		_mainTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
	}
	
	public void FadeOut()
	{
		DoFade(0, 1);
		isVisible = false;
	}
}
