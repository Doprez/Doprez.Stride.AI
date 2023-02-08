using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.FSMs;
public abstract class FSMState : StartupScript
{
	public string Name { get; set; } = Guid.NewGuid().ToString();
	public FSM FiniteStateMachine { get; set; }
	public bool IsDefaultState { get; set; }

	public abstract Task EnterState();
	public abstract Task ExitState();
	public abstract Task UpdateState();

}
