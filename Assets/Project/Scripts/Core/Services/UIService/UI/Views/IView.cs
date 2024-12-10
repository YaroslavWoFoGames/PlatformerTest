using Cysharp.Threading.Tasks;

namespace Platformer.Core
{
	public interface IView
	{
		UniTask Show();
		UniTask Hide();
	}

	public interface IView<in TArgs> : IView
	{
		void Setup(TArgs args);
	}
}