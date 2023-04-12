using Stride.Engine;
using Stride.Core;

namespace Doprez.Stride.AI.Goap;

[ComponentCategory("GOAP")]
public abstract class GoapAction : StartupScript
{
	[DataMember(0)]
	public string ActionName { get; set; } = string.Empty;
	[DataMember(1)]
	public float Cost { get; set; } = 1f;

	[DataMember(10)]
	public Dictionary<string, bool> Preconditions { get; set; } = new();
	[DataMember(11)]
	public Dictionary<string, bool> Postconditions { get; set; } = new();

	public virtual bool CheckProceduralPrecondition(Dictionary<string, bool> prerequisites)
	{
		foreach (var condition in Preconditions)
		{
			if (!prerequisites.TryGetValue(condition.Key, out var value))
			{
				return false;
			}
			if (condition.Value != value)
			{
				return false;
			}
		}
		return true;
	}

	public abstract Task<ActionState> Step();

}
