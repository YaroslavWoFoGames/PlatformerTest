using System.Collections.Generic;
using Platformer.Core;
using UnityEngine;

namespace Platformer.Game.Views
{
	public class CharacterStatsViewArgs : IViewArgs
	{
		public readonly IReadOnlyList<StatViewArgs> StatViewArgs;

		public CharacterStatsViewArgs(IReadOnlyList<StatViewArgs> statViewArgs)
		{
			StatViewArgs = statViewArgs;
		}
	}

	public class CharacterStatsView : ViewWithArgs<CharacterStatsViewArgs>
	{
		[SerializeField] private RectTransform _statsContent = null!;
		[SerializeField] private StatView _prefab = null!;

		protected override void OnSetup()
		{
			DeleteAllDynamicViews();
			foreach (var arg in Args.StatViewArgs)
			{
				CreateDynamicView(arg, _prefab, _statsContent);
			}

			base.OnSetup();
		}
	}
}