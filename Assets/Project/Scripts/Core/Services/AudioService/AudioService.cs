using Platformer.Game.Services;
using UnityEngine;

namespace Platformer.Core
{
	// TODO mini version - не стал запариваться с этим сервисом, так это не нужно

	public class AudioService : IAudioService
	{
		private readonly AudioConfig _audioConfig;
		private readonly AudioSource _audioSource;

		public AudioService(IGameConfigurationService gameConfigurationService, AudioSource oneShotAudioSource)
		{
			_audioConfig = gameConfigurationService.GameConfiguration.AudioConfig;
			_audioSource = oneShotAudioSource;
		}

		public void PlayOneShotById(AudioOneShotType type)
		{
			_audioSource.clip = _audioConfig.GetAudioClipById(type);
			_audioSource.Play();
		}
	}
}