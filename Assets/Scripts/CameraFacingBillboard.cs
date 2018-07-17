using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class CameraFacingBillboard : MonoBehaviour
{
	private Camera camera;

	void Start()
	{
		camera = Camera.main;
	}

	void Update ()
	{
		var rot = camera.transform.rotation;
		rot.x = 0;
		transform.LookAt(
			transform.position + rot * Vector3.forward,
			Vector3.up
		);
	}
}