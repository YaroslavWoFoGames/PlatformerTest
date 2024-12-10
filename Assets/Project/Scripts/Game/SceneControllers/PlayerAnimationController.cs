using Platformer.Core.SceneControllers;
using UnityEngine;

namespace Platformer.Game
{
	public class PlayerAnimationController : SceneController
	{
		private static readonly int RunName = Animator.StringToHash("PlayerRun");
		private static readonly int FireName = Animator.StringToHash("PlayerFire");
		private static readonly int IdleName = Animator.StringToHash("PlayerIdle");

		private static readonly int FireSpeedName = Animator.StringToHash("Speed");
		
		[SerializeField] private Animator _animator = null!;

		public void PlayFire()
		{
			_animator.Play(FireName);
		}

		public void PlayIdle()
		{
			_animator.Play(IdleName);
		}

		public void PlayRun()
		{
			_animator.Play(RunName);
		}

		public void SetFireSpeed(float multiplier)
		{
			_animator.SetFloat(FireSpeedName, multiplier);
		}
	}
}