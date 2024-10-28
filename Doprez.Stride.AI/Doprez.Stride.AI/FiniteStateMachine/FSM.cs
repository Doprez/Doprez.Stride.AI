using Stride.Core;
using Stride.Engine;

namespace Doprez.Stride.AI.FiniteStateMachine;
public class FSM
{
	public FSMState? CurrentState { get; protected set; }

	/// <summary>
	/// Runs the current state of the FSM. Returns the state of the action.
	/// <para>
	/// Returns <see cref="ActionState.Failure"/> if no action is available.
	/// </para>
	/// </summary>
	/// <returns></returns>
	public virtual void Execute()
	{
		CurrentState?.ExecuteState();
	}

	/// <summary>
	/// Exits the <see cref="CurrentState"/> and sets the <see cref="CurrentState"/> to the given <paramref name="state"/>.
	/// </summary>
	/// <param name="state"></param>
	public void SetCurrentState(FSMState state)
	{
		CurrentState?.ExitState();
		CurrentState = state;
		CurrentState?.EnterState();
	}

	/// <summary>
	/// Exits the current state and sets the <see cref="CurrentState"/> to null.
	/// </summary>
	public void CancelCurrentState()
	{
		CurrentState?.ExitState();
		CurrentState = null;
	}
}
