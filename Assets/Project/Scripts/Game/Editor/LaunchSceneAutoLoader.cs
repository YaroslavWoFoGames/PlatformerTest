using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Game.Editor
{
	[InitializeOnLoad]
	public static class LaunchSceneAutoLoader
	{
		private const string LAUNCH_SCENE_PATH = "Assets/Project/Scenes/Launch.unity";
		private const string PREFS_KEY = "P_S";

		static LaunchSceneAutoLoader()
		{
			EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
		}

		private static void HandleOnPlayModeChanged(PlayModeStateChange state)
		{
			if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			{
				if (SceneManager.GetActiveScene().buildIndex == 0)
				{
					return;
				}

				var path = SceneManager.GetActiveScene().path;
				EditorPrefs.SetString(PREFS_KEY, path);
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					try
					{
						EditorSceneManager.OpenScene(LAUNCH_SCENE_PATH);
					}
					catch (Exception e)
					{
						Debug.LogError($"Cannot load scene: {LAUNCH_SCENE_PATH}");
						EditorApplication.isPlaying = false;
					}
				}
				else
				{
					EditorApplication.isPlaying = false;
				}
			}

			if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				var path = EditorPrefs.GetString(PREFS_KEY);
				try
				{
					EditorSceneManager.OpenScene(path);
				}
				catch
				{
					Debug.LogError($"Cannot load scene: {LAUNCH_SCENE_PATH}");
				}
			}
		}
	}
}