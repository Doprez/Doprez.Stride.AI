using Stride.Core;
using Stride.Engine;

namespace Doprez.Stride.AI.FSMs;

/// <summary>
/// Useless for the moment
/// </summary>
public abstract class AsyncFSM : AsyncScript
{
	/// <summary>
	/// All states that can be accessed by the FSM
	/// </summary>
	[DataMemberIgnore]
	public Dictionary<int, AsyncFSMState> States = new();

	protected AsyncFSMState? currentState;

	public void Add(int id, AsyncFSMState state)
	{
		States.Add(id, state);
	}

	public abstract void Initialize();
	public abstract Task AsyncUpdate();

	public override async Task Execute()
	{
		Initialize();
		while (Game.IsRunning)
		{
			await AsyncUpdate();
			await currentState.UpdateState();
		}
	}

	public AsyncFSMState? GetActiveState()
	{
		return currentState;
	}

	public AsyncFSMState GetState(int id)
	{
		return States[id];
	}

	public void SetCurrentState(AsyncFSMState state)
	{
		currentState?.ExitState();

		currentState = state;

		currentState?.EnterState();
	}

	public void SetCurrentState(int stateIndex)
	{
		currentState?.ExitState();

		currentState = GetState(stateIndex);

		currentState?.EnterState();
	}

}
