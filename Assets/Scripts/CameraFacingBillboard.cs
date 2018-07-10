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
		transform.LookAt(Camera.main.transform.position, Vector3.up);
	}
}