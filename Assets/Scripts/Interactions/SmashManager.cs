using Interactions;
using UnityEngine;


public class SmashManager : MonoBehaviour
{

	public Smashable BigBell;
	public Smashable MediumBell;
	public Smashable SmallBell;
	private const float SlowMotionSpeed = 0.5f;
	
	public void Smash ()
	{
		BigBell.Smash();
		MediumBell.SmashAfter(1.2f);
		SmallBell.SmashAfter(1.8f);
	}


}
