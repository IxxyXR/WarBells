using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCycler : MonoBehaviour
{
	void Update ()
	{
        if (Input.GetKeyDown("space"))
        {
	        int nextSceneIndex = 0;
	        var scene = SceneManager.GetActiveScene();
	        var scenes = SceneManager.GetAllScenes();
	        for (int i = 0; i < scenes.Length; i++)
	        {
		        if (scenes[i].name == scene.name)
		        {
			        nextSceneIndex = (i + 1) % scenes.Length;
		        }
	        }
	        SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
