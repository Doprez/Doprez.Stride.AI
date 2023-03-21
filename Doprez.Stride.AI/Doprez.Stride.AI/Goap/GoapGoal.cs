using MountainGoap;
using Stride.Core;
using Stride.Engine;

namespace StrideMountainGoap.Code.Goap;

public abstract class GoapGoal : StartupScript
{
	[DataMember(0)]
	public string GoalName { get; set; } = "";
	[DataMember(1)]
	public int Weight { get; set; } = 1;

	[DataMember(10)]
	public virtual Dictionary<string, bool> DesiredGoals { get; set; } = new Dictionary<string, bool>();

	private Goal _goal;

	public override void Start()
	{

		InitializeGoal();
	}

	public Goal GetGoal() { return _goal; }

	private void InitializeGoal()
	{
		_goal = new Goal
		(
			name: GoalName,
			desiredState: GetDesires(),
			weight: Weight
		);
	}

	private Dictionary<string, object> GetDesires()
	{
		var desires = new Dictionary<string, object>();
		foreach (var goal in DesiredGoals)
		{
			desires.Add(goal.Key, goal.Value);
		}
		return desires;
	}
}
