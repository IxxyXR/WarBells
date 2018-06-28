using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardInput : MonoBehaviour
{

	public int sceneNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
	        SceneManager.LoadScene(sceneNumber);
	        sceneNumber++;
	        sceneNumber %= SceneManager.sceneCount;
        }
    }
}
