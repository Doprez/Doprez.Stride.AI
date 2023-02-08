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
	public Dictionary<int, FSMState> States = new Dictionary<int, FSMState>();

	protected FSMState _currentState;

	public void Add(int id, FSMState state)
	{
		States.Add(id, state);
	}

	public override async Task Execute()
	{
		while (Game.IsRunning)
		{
			await _currentState.UpdateState();
		}
	}

	public FSMState GetActiveState()
	{
		return _currentState;
	}

	public FSMState GetState(int id)
	{
		return States[id];
	}

	public void SetCurrentState(FSMState state)
	{
		if (_currentState != null)
		{
			_currentState.ExitState();
		}

		_currentState = state;

		if (_currentState != null)
		{
			_currentState.EnterState();
		}
	}

	public void SetCurrentState(int stateIndex)
	{
		if (_currentState != null)
		{
			_currentState.ExitState();
		}

		_currentState = GetState(stateIndex);

		if (_currentState != null)
		{
			_currentState.EnterState();
		}
	}

}
