using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class CameraFacingBillboard : MonoBehaviour
{
	private Camera cam;

	void Start()
	{
		cam = Camera.main;
	}

	void Update ()
	{
		var rot = cam.transform.rotation;
		rot.x = 0;
		transform.LookAt(
			transform.position + rot * Vector3.forward,
			Vector3.up
		);
	}
}