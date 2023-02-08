using BulletSharp;
using Stride.Core;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.GOAP;
public abstract class GoapAgent : AsyncScript
{
	[DataMember(0)]
	public string AgentName { get; set; } = string.Empty;
	//Need list of Actions To be performed
	public List<GoapAction> Actions { get; set; } = new();
	//need input state that should be modified
	public GoapState State { get; set; } = new();

	private Queue<GoapAction>? _plan;


	public Queue<GoapAction> Plan(Dictionary<string, bool> worldState, Dictionary<string, bool> goal)
	{
		// reset the plan
		_plan.Clear();

		// Create a new list of actions to check
		List<GoapAction> usableActions = new List<GoapAction>(Actions);

		// Check each action to see if its preconditions are met
		foreach (GoapAction action in usableActions)
		{
			action.Reset();
			foreach (KeyValuePair<string, bool> precondition in action.PreConditions)
			{
				if (worldState.ContainsKey(precondition.Key) && worldState[precondition.Key] == precondition.Value)
				{
					action.IsUsable = true;
				}
				else
				{
					action.IsUsable = false;
					break;
				}
			}
		}

		// Sort the list of usable actions
		usableActions.Sort((x, y) => x.Cost.CompareTo(y.Cost));

		// Create a dictionary to store the state of the world after each action
		Dictionary<string, bool> currentState = new Dictionary<string, bool>(worldState);

		// Create a list to store the actions taken
		List<GoapAction> actionPlan = new List<GoapAction>();

		// loop through the list of usable actions and try to achieve the goal
		foreach (GoapAction action in usableActions)
		{
			// Check if the action can be performed
			if (action.IsUsable)
			{
				// Check if the goal has been met
				bool goalMet = true;
				foreach (KeyValuePair<string, bool> goalState in goal)
				{
					if (currentState.ContainsKey(goalState.Key) && currentState[goalState.Key] == goalState.Value)
					{
						continue;
					}
					else
					{
						goalMet = false;
						break;
					}
				}

				// If the goal has been met, return the plan
				if (goalMet)
				{
					foreach (GoapAction a in actionPlan)
					{
						_plan.Enqueue(a);
					}
					return _plan;
				}
				else
				{
					// Apply the effects of the action to the current state
					foreach (KeyValuePair<string, bool> effect in action.PostConditions)
					{
						currentState[effect.Key] = effect.Value;
					}
					actionPlan.Add(action);
				}
			}
		}

		// If a plan cannot be found, return an empty queue
		return new Queue<GoapAction>();
	}

}
