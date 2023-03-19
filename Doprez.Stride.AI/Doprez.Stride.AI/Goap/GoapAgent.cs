using MountainGoap;
using Stride.Core;
using Stride.Engine;
using Action = MountainGoap.Action;

namespace StrideMountainGoap.Code.Goap;
public abstract class GoapAgent : SyncScript
{
	[DataMember(0)]
	public string AgentName { get; set; } = "";

	[DataMember(10)]
	public List<GoapAction> GoapActions { get; set; } = new List<GoapAction>();
	[DataMember(11)]
	public List<GoapGoal> GoapGoals { get; set; } = new List<GoapGoal>();
	[DataMember(12)]
	public Dictionary<string, bool> CurrentState { get; set; } = new Dictionary<string, bool>();

	protected Agent? _agent;

	public override void Start()
	{
		InitializeAgent();
	}

	private void InitializeAgent()
	{
		_agent = new
		(
			name: AgentName,
			state: GetState(),
			goals: GetGoals(),
			actions: GetActions()
		);
	}

	private Dictionary<string, object> GetState()
	{
		var state = new Dictionary<string, object>();
		foreach ( var agentState in CurrentState ) 
		{
			state.Add(agentState.Key, agentState.Value);
		}
		return state;
	}

	private List<BaseGoal> GetGoals()
	{
		var goals = new List<BaseGoal>();
		foreach(var goal in GoapGoals )
		{
			goals.Add(goal.GetGoal());
		}
		return goals;
	}

	private List<Action> GetActions()
	{
		var actions = new List<Action>();
		foreach (var action in GoapActions )
		{
			actions.Add(action.GetAction());
		}
		return actions;
	}

}
