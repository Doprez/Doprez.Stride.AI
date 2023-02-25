
namespace Doprez.Stride.AI.FSMs;
public abstract class FSMState
{
	public string Name { get; set; } = Guid.NewGuid().ToString();
	public FSM? FiniteStateMachine { get; set; }
	public bool IsDefaultState { get; set; }

	public abstract void EnterState();
	public abstract void ExitState();
	public abstract void UpdateState();
}
