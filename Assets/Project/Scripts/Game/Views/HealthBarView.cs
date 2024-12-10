using DG.Tweening;
using Platformer.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Game.Views
{
	public class HealthBarViewArgs : IViewArgs
	{
		public readonly IReadOnlyReactiveProperty<float> Value;

		public HealthBarViewArgs(IReadOnlyReactiveProperty<float> value)
		{
			Value = value;
		}
	}

	public class HealthBarView : ViewWithArgs<HealthBarViewArgs>
	{
		[SerializeField] private Image _progressBar = null;

		protected override void OnSetup()
		{
			Args.Value.Subscribe(HandleChangeValue).AddTo(CompositeDisposableDeinitialize);
			base.OnSetup();
		}

		private void HandleChangeValue(float value)
		{
			_progressBar.DOFillAmount(value, 0.5f);
		}
	}
}