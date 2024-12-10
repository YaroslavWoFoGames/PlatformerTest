using Platformer.Game.Services;

namespace Platformer.Core
{
	public interface IAudioService
	{
		void PlayOneShotById(AudioOneShotType type);
	}
}