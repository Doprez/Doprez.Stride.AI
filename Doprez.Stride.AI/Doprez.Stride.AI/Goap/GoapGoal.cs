using Stride.Core;
using Stride.Engine;

namespace Doprez.Stride.AI.Goap;

[DataContract(nameof(GoapGoal))]
[ComponentCategory("GOAP")]
[Display("GoapGoal")]
public class GoapGoal : StartupScript
{
	public Dictionary<string, bool> Goal { get; set; } = new();
}
