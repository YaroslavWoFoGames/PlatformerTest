using System.Globalization;
using Platformer.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Platformer.Game.Views
{
	public class StatViewArgs : IViewArgs
	{
		public readonly IReadOnlyReactiveProperty<int> Value;

		public StatViewArgs(IReadOnlyReactiveProperty<int> value)
		{
			Value = value;
		}
	}

	public class StatView : ViewWithArgs<StatViewArgs>
	{
		[SerializeField] private TextMeshProUGUI _value = null!;

		protected override void OnSetup()
		{
			Args.Value.Subscribe(HandleUpdateStatValue).AddTo(CompositeDisposableDeactivate);
			base.OnSetup();
		}

		private void HandleUpdateStatValue(int value)
		{
			_value.text = value.ToString(CultureInfo.InvariantCulture);
		}
	}
}