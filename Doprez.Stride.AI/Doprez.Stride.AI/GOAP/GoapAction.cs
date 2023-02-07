using Stride.Engine;

namespace Doprez.Stride.AI.GOAP;
public abstract class GoapAction : StartupScript
{

	public Dictionary<string, bool> PreConditions = new();
	public Dictionary<string, bool> PostConditions = new();

	/// <summary>
	/// Returns true if the action was successful
	/// </summary>
	/// <returns></returns>
	public abstract Task<bool> RunAction();
}
