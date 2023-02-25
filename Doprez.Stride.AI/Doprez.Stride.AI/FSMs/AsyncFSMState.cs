
namespace Doprez.Stride.AI.FSMs;
public abstract class AsyncFSMState
{
	public string Name { get; set; } = Guid.NewGuid().ToString();
	public AsyncFSM? FiniteStateMachine { get; set; }
	public bool IsDefaultState { get; set; }

	public abstract Task EnterState();
	public abstract Task ExitState();
	public abstract Task UpdateState();

}
