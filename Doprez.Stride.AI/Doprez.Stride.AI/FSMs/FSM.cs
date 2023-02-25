using Stride.Core;
using Stride.Engine;

namespace Doprez.Stride.AI.FSMs;
public abstract class FSM : SyncScript
{

	/// <summary>
	/// All states that can be accessed by the FSM
	/// </summary>
	[DataMemberIgnore]
	public Dictionary<int, FSMState> States = new();

	protected FSMState? currentState;

	public override void Update()
	{
		UpdateFSM();
		currentState?.UpdateState();
	}

	public void Add(int id, FSMState state)
	{
		States.Add(id, state);
	}

	public abstract void UpdateFSM();

	public FSMState GetActiveState()
	{
		return currentState;
	}

	public FSMState GetState(int id)
	{
		return States[id];
	}

	public void SetCurrentState(FSMState state)
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
