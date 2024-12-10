using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Platformer.Core;

namespace Platformer.Game.States
{
	public class ExitGameState : GameState
	{
		protected override void OnEnter()
		{
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				EditorApplication.isPlaying = false;
			}
#else
            Application.Quit();
#endif
		}

		protected override void OnExit() { }
	}
}