using Stride.Engine;

namespace Doprez.Stride.AI.Utility;
public abstract class UtilityAgent : StartupScript
{

	/// <summary>
	/// State that can be used for planning and decision making.
	/// </summary>
	public Dictionary<string, float> AgentState = [];

	/// <summary>
	/// All actions that the agent can perform and plan for.
	/// </summary>
	public List<UtilityAction> Actions = [];

	public UtilityAction CurrentAction { get; protected set; }

	public virtual UtilityAction GetBestAction()
	{
		UtilityAction bestAction = null;
		float bestUtility = float.MinValue;

		foreach (var action in Actions)
		{
			float utility = action.CalculateUtility(this);
			if (utility > bestUtility)
			{
				bestUtility = utility;
				bestAction = action;
			}
		}

		return bestAction;
	}

	/// <summary>
	/// Will perform the best action based on the utility of the actions. If the current action is done or null, it will get the best action.
	/// <para>
	/// Returns <see cref="ActionState.Failure"/> if no action is available.
	/// </para>
	/// </summary>
	public virtual ActionState PerformBestAction()
	{
		CheckIfBestActionChanged();

		if (CurrentAction == null)
		{
			return ActionState.Failure;
		}

		var status = CurrentAction.Execute(this);
		switch (status)
		{
			case ActionState.Success:
			case ActionState.Failure:
			case ActionState.Impossible:
			case ActionState.Finished:
				CurrentAction = GetBestAction();
				break;
			case ActionState.Running:
				break;
			default:
				break;
		}

		return status;
	}

	/// <summary>
	/// Updates the current action if the best action has changed. Returns true if the best action has changed.
	/// </summary>
	/// <returns></returns>
	public bool CheckIfBestActionChanged()
	{
		var bestAction = GetBestAction();
		if (bestAction != CurrentAction)
		{
			CurrentAction?.CancelAction(this);
			CurrentAction = bestAction;
			return true;
		}

		return false;
	}
}
