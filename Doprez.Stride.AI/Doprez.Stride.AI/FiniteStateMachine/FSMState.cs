
using Stride.Core;
using Stride.Engine;

namespace Doprez.Stride.AI.FiniteStateMachine;
public abstract class FSMState : StartupScript
{
	public string Name { get; set; } = Guid.NewGuid().ToString();
	public FSM? StateMachine { get; set; }
	public bool IsDefaultState { get; set; }

	public virtual void Initialize(IServiceRegistry services) { }
	public abstract void EnterState();
	public abstract void ExitState();
	public abstract void ExecuteState();
}
