using System;
using Platformer.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Game.Views
{
	public class DiedPanelArs : IViewArgs
	{
		public readonly Action CallbackClickExit;
		public readonly Action CallbackClickRestart;

		public DiedPanelArs(Action callbackClickExit, Action callbackClickRestart)
		{
			CallbackClickExit = callbackClickExit;
			CallbackClickRestart = callbackClickRestart;
		}
	}

	public class DiedPanel : ViewWithArgs<DiedPanelArs>, IPanel
	{
		[SerializeField] private Button _buttonExit = null!;
		[SerializeField] private Button _buttonRestart = null!;

		protected override void OnActivate()
		{
			_buttonExit.onClick.AddListener(HandleButtonClickExit);
			_buttonRestart.onClick.AddListener(HandleButtonClickRestart);
			base.OnActivate();
		}

		protected override void OnDeactivate()
		{
			_buttonExit.onClick.RemoveListener(HandleButtonClickExit);
			_buttonRestart.onClick.RemoveListener(HandleButtonClickRestart);
			base.OnDeactivate();
		}

		private void HandleButtonClickExit()
		{
			Args?.CallbackClickExit?.Invoke();
		}

		private void HandleButtonClickRestart()
		{
			Args?.CallbackClickRestart?.Invoke();
		}
	}
}