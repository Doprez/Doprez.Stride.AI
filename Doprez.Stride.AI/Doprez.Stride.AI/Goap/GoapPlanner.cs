
using ServiceWire;
using System.Collections.Generic;

namespace Doprez.Stride.AI.Goap;

/// <summary>
/// Taken from HappieGoap's Unity example and adapted for Stride.
/// </summary>
public class GoapPlanner
{

	/// <summary>
	/// Plan what sequence of actions can fulfill the goal.
	/// Returns null if a plan could not be found, or a list of the actions
	/// that must be performed, in order, to fulfill the goal.
	/// </summary>
	public Queue<GoapAction> Plan(GoapAgent agent)
	{

		// check what actions can run using their checkProceduralPrecondition
		HashSet<GoapAction> usableActions = new();
		foreach (GoapAction a in agent.AvailableActions)
		{
			usableActions.Add(a);
		}

		// build up the tree and record the leaf nodes that provide a solution to the goal.
		List<Node> leaves = new();

		// build graph
		Node start = new Node(null, 0, agent.AgentState, null);
		bool success = BuildGraph(start, leaves, usableActions, agent.CurrentGoal.Goal);

		if (!success)
		{
			return null;
		}

		// get the cheapest leaf
		Node cheapest = null;
		foreach (Node leaf in leaves)
		{
			if (cheapest == null)
				cheapest = leaf;
			else
			{
				if (leaf.RunningCost < cheapest.RunningCost)
					cheapest = leaf;
			}
		}

		// get its node and work back through the parents
		List<GoapAction> result = new List<GoapAction>();
		Node n = cheapest;
		while (n != null)
		{
			if (n.Action != null)
			{
				result.Insert(0, n.Action); // insert the action in the front
			}
			n = n.Parent;
		}
		// we now have this action list in correct order

		Queue<GoapAction> queue = new Queue<GoapAction>();
		foreach (GoapAction a in result)
		{
			queue.Enqueue(a);
		}

		// hooray we have a plan!
		return queue;
	}




	/// <summary>
	/// Returns true if at least one solution was found.
	/// The possible paths are stored in the leaves list.Each leaf has a
	/// 'runningCost' value where the lowest cost will be the best action sequence.
	/// </summary>
	protected bool BuildGraph(Node parent, List<Node> leaves, HashSet<GoapAction> usableActions, Dictionary<string, bool> goal)
	{
		bool foundOne = false;

		// go through each action available at this node and see if we can use it here
		foreach (GoapAction action in usableActions)
		{

			// if the parent state has the conditions for this action's preconditions, we can use it here
			if (InState(action.Preconditions, parent.State))
			{

				// apply the action's effects to the parent state
				Dictionary<string, bool> currentState = PopulateState(parent.State, action.Postconditions);
				//Debug.Log(GoapAgent.prettyPrint(currentState));
				Node node = new Node(parent, parent.RunningCost + action.Cost, currentState, action);

				if (GoalInState(goal, currentState))
				{
					// we found a solution!
					leaves.Add(node);
					foundOne = true;
				}
				else
				{
					// test all the remaining actions and branch out the tree
					HashSet<GoapAction> subset = ActionSubset(usableActions, action);
					bool found = BuildGraph(node, leaves, subset, goal);
					if (found)
						foundOne = true;
				}


			}
		}

		return foundOne;
	}

	/// <summary>
	/// Create a subset of the actions excluding the removeMe one. Creates a new set.
	/// </summary>
	protected HashSet<GoapAction> ActionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
	{
		HashSet<GoapAction> subset = new HashSet<GoapAction>();
		foreach (GoapAction a in actions)
		{
			if (!a.Equals(removeMe))
				subset.Add(a);
		}
		return subset;
	}

	/// <summary>
	///  Checks if at least one goal is met. 
	/// </summary>
	protected bool GoalInState(Dictionary<string, bool> test, Dictionary<string, bool> state)
	{
		bool match = false;
		foreach (KeyValuePair<string, bool> t in test)
		{
			foreach (KeyValuePair<string, bool> s in state)
			{
				if (s.Equals(t))
				{
					match = true;
					break;
				}
			}
		}
		return match;
	}

	/// <summary>
	/// Check that all items in 'test' are in 'state'. If just one does not match or is not there
	/// then this returns false.
	/// </summary>
	protected bool InState(Dictionary<string, bool> test, Dictionary<string, bool> state)
	{
		bool allMatch = true;
		foreach (KeyValuePair<string, bool> t in test)
		{
			bool match = false;
			foreach (KeyValuePair<string, bool> s in state)
			{
				if (s.Equals(t))
				{
					match = true;
					break;
				}
			}
			if (!match)
				allMatch = false;
		}
		return allMatch;
	}

	/// <summary>
	/// Apply the stateChange to the currentState
	/// </summary>
	protected Dictionary<string, bool> PopulateState(Dictionary<string, bool> currentState, Dictionary<string, bool> stateChange)
	{
		Dictionary<string, bool> state = new Dictionary<string, bool>();
		// copy the KVPs over as new objects
		foreach (KeyValuePair<string, bool> s in currentState)
		{
			state.Add(s.Key, s.Value);
		}

		foreach (KeyValuePair<string, bool> change in stateChange)
		{
			// if the key exists in the current state, update the Value
			bool exists = false;

			foreach (KeyValuePair<string, bool> s in state)
			{
				if (s.Key.Equals(change.Key))
				{
					exists = true;
					break;
				}
			}

			if (exists)
			{
				state.Remove(change.Key);
				KeyValuePair<string, bool> updated = new KeyValuePair<string, bool>(change.Key, change.Value);
				state.Add(updated.Key, updated.Value);
			}
			// if it does not exist in the current state, add it
			else
			{
				state.Add(change.Key, change.Value);
			}
		}
		return state;
	}

	/// <summary>
	/// Used for building up the graph and holding the running costs of actions.
	/// </summary>
	protected class Node
	{
		public Node Parent;
		public float RunningCost;
		public Dictionary<string, bool> State;
		public GoapAction Action;

		public Node(Node parent, float runningCost, Dictionary<string, bool> state, GoapAction action)
		{
			Parent = parent;
			RunningCost = runningCost;
			State = state;
			Action = action;
		}
	}

}
