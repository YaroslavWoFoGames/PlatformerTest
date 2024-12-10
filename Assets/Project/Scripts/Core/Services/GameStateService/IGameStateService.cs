namespace Platformer.Core
{
	public interface IGameStateService
	{
		public void SwitchState<T>(bool shouldRemoveFromCache = false) where T : IGameState, new();

		public void SwitchState<TState, TContext>(TContext context, bool shouldRemoveFromCache = false)
			where TState : IGameState<TContext>, new()
			where TContext : IGameStateContext;
	}
}