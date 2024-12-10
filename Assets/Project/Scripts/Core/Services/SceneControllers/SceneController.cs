using UniRx;
using UnityEngine;

namespace Platformer.Core.SceneControllers
{
	public abstract class SceneController : MonoBehaviour
	{
		protected readonly CompositeDisposable CompositeDisposableOnDeactivate = new();

		private void OnEnable()
		{
			OnActivate();
		}

		private void OnDisable()
		{
			OnDeactivate();
			CompositeDisposableOnDeactivate?.Dispose();
		}

		private void Update()
		{
			OnUpdate(Time.deltaTime);
		}

		protected virtual void OnUpdate(float deltaTime) { }
		protected virtual void OnActivate() { }

		protected virtual void OnDeactivate() { }
	}
}