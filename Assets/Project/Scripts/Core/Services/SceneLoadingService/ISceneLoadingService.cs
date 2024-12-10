using Cysharp.Threading.Tasks;

namespace Platformer.Core
{
	public interface ISceneLoadingService
	{
		UniTask LoadSceneAsync(string sceneName);
	}
}