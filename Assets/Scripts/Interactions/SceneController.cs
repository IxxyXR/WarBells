﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactions
{
	public class SceneController : MonoBehaviour
	{
		void Start()
		{
			//Camera.main.depthTextureMode = DepthTextureMode.Depth;
			QualitySettings.vSyncCount = 0;
			
		}

		void Update()
		{
			if (Input.GetKeyDown("space") || GvrControllerInput.AppButtonDown)
			{
				NextScene();
			}
		}

		public void NextScene()
		{
			int nextSceneIndex = 0;
			int sceneCount = SceneManager.sceneCountInBuildSettings;
			var scene = SceneManager.GetActiveScene();
			for (int i = 0; i < sceneCount; i++)
			{
				if (SceneManager.GetSceneByBuildIndex(i).name == scene.name)
				{
					nextSceneIndex = (i + 1) % sceneCount;
				}
			}

			SceneManager.LoadScene(nextSceneIndex);
		}

	}
}
