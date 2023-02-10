using SharpDX.Direct3D11;
using Stride.Core;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.FSMs;

/// <summary>
/// Useless for the moment
/// </summary>
public abstract class FSM : AsyncScript
{
	/// <summary>
	/// All states that can be accessed by the FSM
	/// </summary>
	[DataMemberIgnore]
	public Dictionary<int, FSMState> States = new();

	protected FSMState? currentState;

	public void Add(int id, FSMState state)
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
