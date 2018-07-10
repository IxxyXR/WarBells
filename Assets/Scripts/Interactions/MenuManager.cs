using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactions
{
	public class MenuManager : MonoBehaviour {

		public void LoadScene(string SceneName)
		{
			SceneManager.LoadScene(SceneName);
		}
	}
}
