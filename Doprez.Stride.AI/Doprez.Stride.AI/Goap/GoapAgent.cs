using Doprez.Stride.AI;
using Stride.Core;
using Stride.Engine;

namespace Doprez.Stride.AI.Goap;

[DataContract(nameof(GoapAgent))]
[ComponentCategory("GOAP")]
[Display("GoapAgent")]
public class GoapAgent : StartupScript
{
	public Dictionary<string, bool> AgentState { get; set; } = new();
	public List<GoapAction> AvailableActions { get; set; } = new();
	public GoapGoal CurrentGoal { get; set; }

	private Queue<GoapAction> _actions = new();
	private readonly GoapPlanner _planner = new();
	private GoapAction _currentAction;

	public override void Start()
	{
		base.Start();

		MakeNewPlan();
	}

	public async Task RunPlannedActions()
	{
		if (_currentAction != null)
		{
			var actionState = await _currentAction.Step();
			switch (actionState)
			{
				case ActionState.Success:
					UpdateStateWithCurrentAction();
					MoveToNextAction();
					break;
				case ActionState.Failure:
					MakeNewPlan();
					break;
				case ActionState.Impossible:
					MakeNewPlan();
					break;
				case ActionState.Finished:
					MakeNewPlan();
					break;
				case ActionState.Running:
					break;
				default:
					break;
			}
		}
	}

	private void MoveToNextAction()
	{
		if (_actions.Count > 0)
		{
			_actions.Dequeue();
			_currentAction = _actions.TryPeek(out var newAction) ? newAction : null;
		}
	}

	private void UpdateStateWithCurrentAction()
	{
		foreach (var effect in _currentAction.Postconditions)
		{
			if (AgentState.TryGetValue(effect.Key, out var state))
			{
				AgentState[effect.Key] = state;
			}
			else
			{
				AgentState.TryAdd(effect.Key, effect.Value);
			}
		}
	}

	public void MakeNewPlan()
	{
		_actions.Clear();
		_actions = _planner.Plan(this);
		if (_actions.Count > 0)
		{
			_currentAction = _actions.Peek();
		}
	}

	public string GetPlanAsString()
	{
		string plan = string.Empty;
		foreach (var action in _actions)
		{
			plan += $"{action.ActionName} -> ";
		}
		return plan;
	}

}
