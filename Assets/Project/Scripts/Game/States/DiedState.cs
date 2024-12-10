using Platformer.Core;
using Platformer.Game.Views;

namespace Platformer.Game.States
{
	public class DiedState : GameState
	{
		protected override void OnEnter()
		{
			var args = new DiedPanelArs(HandleExitGame, HandleRestartGame);
			UIService.CreateView<DiedPanel, DiedPanelArs>(args);
		}

		protected override void OnExit()
		{
			UIService.HideView<DiedPanel>();
		}

		private void HandleExitGame()
		{
			GameStateService.SwitchState<ExitGameState>();
		}

		private void HandleRestartGame()
		{
			GameStateService.SwitchState<LevelPlayState>();
		}
	}
}